document.addEventListener('DOMContentLoaded', () => {
    const startChatBtn = document.getElementById('start-chat-btn');
    const signInBtn = document.getElementById('sign-in-btn');

    startChatBtn.addEventListener('click', (e) => {
        e.preventDefault();
        window.location.href = 'call.html';
    });

    signInBtn.addEventListener('click', (e) => {
        e.preventDefault();
        window.location.href = 'login.html';
    });
});