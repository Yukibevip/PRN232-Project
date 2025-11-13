document.addEventListener('DOMContentLoaded', () => {
    // SignalR
    const connection = new signalR.HubConnectionBuilder().withUrl("/videoChatHub?userId=" + userId).build();
    let targetId;
    let peer;
    const servers = {
        iceServers: [
            {
                urls: ['stun:stun1.l.google.com:19302'],
            },
            {
                urls: 'turn:numb.viagenie.ca',
                username: 'webrtc@live.com',
                credential: 'muazkh'
            }
        ],
        iceCandidatePoolSize: 10,
    };

    // DOM Elements
    const localVideo = document.getElementById('local-video');
    const remoteVideo = document.getElementById('remote-video');
    const chatMessages = document.getElementById('chat-messages');
    const chatForm = document.getElementById('chat-form');
    const chatInput = document.getElementById('chat-input');
    const nextBtn = document.getElementById('next-btn');
    const homeBtn = document.getElementById('home-btn');
    const searchingOverlay = document.getElementById('searching-overlay');
    const searchingText = document.getElementById('searching-text');
    const muteBtn = document.getElementById('mute-btn');
    const cameraBtn = document.getElementById('camera-btn');
    const cameraOffOverlay = document.getElementById('camera-off-overlay');
    const genderFilter = document.getElementById('gender-filter');
    const countryFilter = document.getElementById('country-filter');

    // State
    let localStream;
    let isMuted = false;
    let isCameraOff = false;
    let conversationInterval;

    // SVG Icons
    const icons = {
        micOn: `<svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M12 18.75a6 6 0 006-6v-1.5m-6 7.5a6 6 0 01-6-6v-1.5m12 0v-1.5a6 6 0 00-6-6v0a6 6 0 00-6 6v1.5m6 15.75a6 6 0 006-6v-1.5" /></svg>`,
        micOff: `<svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M12 18.75a6 6 0 006-6v-1.5m-6 7.5a6 6 0 01-6-6v-1.5m12 0v-1.5a6 6 0 00-6-6v0a6 6 0 00-6 6v1.5m-6.32 8.32l2.122-2.122M15.88 15.88l2.122 2.122m-12-12l12 12" /></svg>`,
        cameraOn: `<svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M15.75 10.5l4.72-4.72a.75.75 0 011.28.53v11.38a.75.75 0 01-1.28.53l-4.72-4.72M4.5 18.75h9a2.25 2.25 0 002.25-2.25v-9a2.25 2.25 0 00-2.25-2.25h-9A2.25 2.25 0 002.25 7.5v9A2.25 2.25 0 004.5 18.75z" /></svg>`,
        cameraOff: `<svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M15.75 10.5l4.72-4.72a.75.75 0 011.28.53v11.38a.75.75 0 01-1.28.53l-4.72-4.72M4.5 18.75h9a2.25 2.25 0 002.25-2.25v-9a2.25 2.25 0 00-2.25-2.25h-9A2.25 2.25 0 002.25 7.5v9A2.25 2.25 0 004.5 18.75z" /><path stroke-linecap="round" stroke-linejoin="round" d="M21 21L3 3" /></svg>`
    };

    // --- Media Functions ---
    async function startMedia() {
        try {
            // Get user media stream
            localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
            // Attach the stream to the local video element
            localVideo.srcObject = localStream;
            // TODO: In a real app, you would send this stream to the peer
        } catch (error) {
            console.error("Error accessing media devices.", error);
            alert("Could not access your camera and microphone. Please check permissions and try again.");
        }
    }

    function toggleMic() {
        if (!localStream) return;
        isMuted = !isMuted;
        localStream.getAudioTracks().forEach(track => track.enabled = !isMuted);
        muteBtn.innerHTML = isMuted ? icons.micOff : icons.micOn;
        muteBtn.classList.toggle('off', isMuted);
        muteBtn.setAttribute('aria-label', isMuted ? 'Unmute Mic' : 'Mute Mic');
    }

    function toggleCamera() {
        if (!localStream) return;
        isCameraOff = !isCameraOff;
        localStream.getVideoTracks().forEach(track => track.enabled = !isCameraOff);
        cameraBtn.innerHTML = isCameraOff ? icons.cameraOff : icons.cameraOn;
        cameraBtn.classList.toggle('off', isCameraOff);
        cameraOffOverlay.classList.toggle('active', isCameraOff);
        cameraBtn.setAttribute('aria-label', isCameraOff ? 'Enable Camera' : 'Disable Camera');
    }

    // --- Chat Functions ---
    const mockMessages = ["Hey, where are you from?", "Cool! I've always wanted to visit.", "What are your hobbies?", "That's awesome! I'm into coding and hiking.", "Nice talking to you!", "Hello!", "How's the weather over there?"];

    function addMessage(text, side) {
        const bubble = document.createElement('div');
        bubble.classList.add('chat-bubble', side);
        bubble.textContent = text;
        chatMessages.appendChild(bubble);
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }

    function simulateConversation() {
        addMessage("You have been connected.", "remote");
        setTimeout(() => addMessage("Hey!", "remote"), 1500);

        conversationInterval = setInterval(() => {
            const randomMsg = mockMessages[Math.floor(Math.random() * mockMessages.length)];
            addMessage(randomMsg, 'remote');
        }, 5000 + Math.random() * 3000);
    }

    function handleNextChat() {
        if (conversationInterval) {
            clearInterval(conversationInterval);
        }

        chatMessages.innerHTML = '';
        searchingOverlay.classList.add('active');

        const gender = genderFilter.options[genderFilter.selectedIndex].text;
        const country = countryFilter.options[countryFilter.selectedIndex].text;
        searchingText.textContent = `Searching for a partner... (${gender}, ${country})`;

        searchingOverlay.classList.remove('active');
        simulateConversation();
        // TODO: Real app logic to find a new peer
        connection.invoke("SearchOthers", userId);
    }

    // --- WebRTC and SignalR Setup ---
    connection.on("ReceivePartnerId", async (fromId, partnerId) => {
        console.log("Received partner ID: " + partnerId);
        targetId = fromId;

        peer = new RTCPeerConnection(servers);
        let remoteStream = new MediaStream();

        peer.onicecandidate = e => {
            console.log("in ice");
            if (e.candidate) {
                console.log("Gửi ICE:", e.candidate);
                connection.invoke("SendIceCandidate", targetId, JSON.stringify(e.candidate));
            }
        };

        peer.ontrack = event => {
            event.streams[0].getTracks().forEach(track => {
                remoteStream.addTrack(track);
            });

            remoteVideo.srcObject = remoteStream;
        };

        localVideo.srcObject = localStream;
        localStream.getTracks().forEach(track => peer.addTrack(track, localStream));

        const offer = await peer.createOffer();
        await peer.setLocalDescription(offer);
        connection.invoke("SendOffer", targetId, JSON.stringify(offer));
    });

    connection.on("ReceiveOffer", async (fromId, offer) => {
        targetId = fromId;
        console.log("Received offer from " + fromId);

        peer = new RTCPeerConnection(servers);
        let remoteStream = new MediaStream();

        peer.onicecandidate = e => {
            console.log("in ice");
            if (e.candidate) {
                console.log("Gửi ICE:", e.candidate);
                connection.invoke("SendIceCandidate", targetId, JSON.stringify(e.candidate));
            }
        };

        peer.ontrack = event => {
            event.streams[0].getTracks().forEach(track => {
                remoteStream.addTrack(track);
            });

            remoteVideo.srcObject = remoteStream;
        };

        localVideo.srcObject = localStream;
        localStream.getTracks().forEach(track => peer.addTrack(track, localStream));

        await peer.setRemoteDescription(new RTCSessionDescription(JSON.parse(offer)));
        const answer = await peer.createAnswer();
        await peer.setLocalDescription(answer);
        connection.invoke("SendAnswer", fromId, JSON.stringify(answer));
    });

    connection.on("ReceiveAnswer", async (fromId, answer) => {
        console.log("Received answer from " + fromId);

        await peer.setRemoteDescription(new RTCSessionDescription(JSON.parse(answer)));
    });

    connection.on("ReceiveIceCandidate", async (fromId, candidate) => {
        console.log("Received candidate from " + fromId);

        await peer.addIceCandidate(new RTCIceCandidate(JSON.parse(candidate)));
    });

    connection.on("PartnerDisconnected", async () => {
        homeBtn.click();
    });

    // --- Event Listeners ---
    muteBtn.addEventListener('click', toggleMic);
    cameraBtn.addEventListener('click', toggleCamera);
    nextBtn.addEventListener('click', handleNextChat);
    homeBtn.addEventListener('click', () => window.location.href = '/Home/index');

    chatForm.addEventListener('submit', (e) => {
        e.preventDefault();
        const text = chatInput.value.trim();
        if (text) {
            addMessage(text, 'local');
            chatInput.value = '';
            // TODO: Send message to peer
        }
    });

    // --- Initialization ---
    async function initialize() {
        await startMedia();
        connection.start();
        chatInput.focus();
        simulateConversation();
    }

    initialize();
});