document.addEventListener('DOMContentLoaded', () => {
    const mockFriends = [
        { id: 1, name: 'Alice', online: true, lastMessage: 'See you later!', avatar: 'https://picsum.photos/id/237/50' },
        { id: 2, name: 'Bob', online: false, lastMessage: 'Okay, sounds good.', avatar: 'https://picsum.photos/id/238/50' },
        { id: 3, name: 'Charlie', online: true, lastMessage: 'Haha, that\'s funny!', avatar: 'https://picsum.photos/id/239/50' },
        { id: 4, name: 'Diana', online: true, lastMessage: 'I am on my way.', avatar: 'https://picsum.photos/id/240/50' },
        { id: 5, name: 'Ethan', online: false, lastMessage: 'Let me check and get back to you.', avatar: 'https://picsum.photos/id/241/50' },
        { id: 6, name: 'Fiona', online: false, lastMessage: 'Happy birthday!', avatar: 'https://picsum.photos/id/242/50' },
        { id: 7, name: 'George', online: true, lastMessage: 'Can you send me the file?', avatar: 'https://picsum.photos/id/243/50' },
        { id: 8, name: 'Hannah', online: false, lastMessage: 'I will be there in 5 minutes.', avatar: 'https://picsum.photos/id/244/50' },
        { id: 9, name: 'Ian', online: true, lastMessage: 'Great, thanks!', avatar: 'https://picsum.photos/id/247/50' },
        { id: 10, name: 'Jane', online: true, lastMessage: 'Let\'s catch up tomorrow.', avatar: 'https://picsum.photos/id/248/50' },
    ];

    const friendsList = document.getElementById('friends-list');
    const searchInput = document.getElementById('search-input');
    const noResults = document.getElementById('no-results');

    function renderFriends(friends = mockFriends) {
        friendsList.innerHTML = '';
        if (friends.length === 0) {
            noResults.style.display = 'block';
            return;
        }
        noResults.style.display = 'none';

        friends.forEach(friend => {
            const li = document.createElement('li');
            li.className = 'friend-item';
            li.dataset.id = friend.id;

            li.innerHTML = `
                            <img src="${friend.avatar}" alt="${friend.name}'s avatar" class="avatar">
                            <div class="friend-info">
                                <div class="friend-name">
                                    ${friend.name}
                                    <span class="status-badge ${friend.online ? 'online' : 'offline'}"></span>
                                </div>
                                <p class="last-message">${friend.lastMessage}</p>
                            </div>
                            <div class="friend-actions">
                                <button class="action-btn call" aria-label="Call ${friend.name}">
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M2.25 6.75c0 8.284 6.716 15 15 15h2.25a2.25 2.25 0 002.25-2.25v-1.372c0-.516-.351-.966-.852-1.091l-4.423-1.106c-.44-.11-.902.055-1.173.417l-.97 1.293c-.282.376-.769.542-1.21.38a12.035 12.035 0 01-7.143-7.143c-.162-.441.004-.928.38-1.21l1.293-.97c.363-.271.527-.734.417-1.173L6.963 3.102a1.125 1.125 0 00-1.091-.852H4.5A2.25 2.25 0 002.25 4.5v2.25z" /></svg>
                                </button>
                                <button class="action-btn message" aria-label="Message ${friend.name}">
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M21.75 6.75v10.5a2.25 2.25 0 01-2.25 2.25h-15a2.25 2.25 0 01-2.25-2.25V6.75m19.5 0A2.25 2.25 0 0019.5 4.5h-15a2.25 2.25 0 00-2.25 2.25m19.5 0v.243a2.25 2.25 0 01-1.07 1.916l-7.5 4.615a2.25 2.25 0 01-2.36 0L3.32 8.91a2.25 2.25 0 01-1.07-1.916V6.75" /></svg>
                                </button>
                                <button class="action-btn remove" aria-label="Remove ${friend.name}">
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0" /></svg>
                                </button>
                            </div>
                        `;
            friendsList.appendChild(li);
        });
    }

    searchInput.addEventListener('input', (e) => {
        const searchTerm = e.target.value.toLowerCase();
        const filteredFriends = mockFriends.filter(friend => friend.name.toLowerCase().includes(searchTerm));
        renderFriends(filteredFriends);
    });

    friendsList.addEventListener('click', (e) => {
        const button = e.target.closest('.action-btn');
        if (!button) return;

        const friendItem = button.closest('.friend-item');
        const friendId = friendItem.dataset.id;
        const friendName = friendItem.querySelector('.friend-name').textContent.trim();

        if (button.classList.contains('call')) {
            // In a real app, this would initiate a call
            window.location.href = `call.html?friendId=${friendId}`;
        }

        if (button.classList.contains('message')) {
            // Redirect to the call page which has a chat interface
            window.location.href = `call.html?friendId=${friendId}&action=chat`;
        }

        if (button.classList.contains('remove')) {
            if (confirm(`Are you sure you want to remove ${friendName} from your friends list?`)) {
                friendItem.style.transition = 'opacity 0.3s ease';
                friendItem.style.opacity = '0';
                setTimeout(() => friendItem.remove(), 300);
                // Here you would also update your data source
            }
        }
    });

    // Initial render
    renderFriends();
});