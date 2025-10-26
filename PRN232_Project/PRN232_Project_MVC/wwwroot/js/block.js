document.addEventListener('DOMContentLoaded', () => {
    let mockBlockedUsers = [
        { id: 101, name: 'Spammer_1', date: '2024-05-10', reason: 'Spamming links', avatar: 'https://picsum.photos/id/301/50' },
        { id: 102, name: 'RudeUser', date: '2024-05-08', reason: 'Inappropriate behavior', avatar: 'https://picsum.photos/id/302/50' },
        { id: 103, name: 'BotAccount', date: '2024-04-22', reason: 'Automated bot', avatar: 'https://picsum.photos/id/304/50' },
    ];

    const blockedList = document.getElementById('blocked-list');
    const noBlocked = document.getElementById('no-blocked');
    const blockedCount = document.getElementById('blocked-count');
    const clearAllBtn = document.getElementById('clear-all-btn');

    // Modal elements
    const modalOverlay = document.getElementById('modal-overlay');
    const modalTitle = document.getElementById('modal-title');
    const modalText = document.getElementById('modal-text');
    const modalCancelBtn = document.getElementById('modal-cancel-btn');
    const modalConfirmBtn = document.getElementById('modal-confirm-btn');
    let confirmCallback = null;

    function renderBlockedList() {
        blockedList.innerHTML = '';
        blockedCount.textContent = `${mockBlockedUsers.length} Blocked Users`;

        if (mockBlockedUsers.length === 0) {
            noBlocked.style.display = 'block';
            blockedList.style.display = 'none';
            clearAllBtn.style.display = 'none';
            return;
        }

        noBlocked.style.display = 'none';
        blockedList.style.display = 'block';
        clearAllBtn.style.display = 'block';

        mockBlockedUsers.forEach(user => {
            const li = document.createElement('li');
            li.className = 'blocked-item';
            li.dataset.id = user.id;
            li.innerHTML = `
                            <img src="${user.avatar}" alt="${user.name}'s avatar" class="avatar">
                            <div class="blocked-info">
                                <div class="blocked-name">${user.name}</div>
                                <div class="blocked-details">Blocked on ${user.date} | Reason: ${user.reason}</div>
                            </div>
                            <button class="btn-unblock" data-name="${user.name}">Unblock</button>
                        `;
            blockedList.appendChild(li);
        });
    }

    function openModal(title, text, onConfirm) {
        modalTitle.textContent = title;
        modalText.textContent = text;
        confirmCallback = onConfirm;
        modalOverlay.style.display = 'flex';
        setTimeout(() => modalOverlay.classList.add('active'), 10);
        modalConfirmBtn.focus();
    }

    function closeModal() {
        modalOverlay.classList.remove('active');
        setTimeout(() => modalOverlay.style.display = 'none', 300);
        confirmCallback = null;
    }

    blockedList.addEventListener('click', e => {
        if (e.target.classList.contains('btn-unblock')) {
            const item = e.target.closest('.blocked-item');
            const userId = parseInt(item.dataset.id);
            const userName = e.target.dataset.name;

            openModal(
                `Unblock ${userName}?`,
                `Are you sure you want to unblock ${userName}? They will be able to contact you again.`,
                () => {
                    mockBlockedUsers = mockBlockedUsers.filter(u => u.id !== userId);
                    renderBlockedList();
                    closeModal();
                }
            );
        }
    });

    clearAllBtn.addEventListener('click', () => {
        openModal(
            'Clear All Blocked Users?',
            'This will unblock everyone on this list. This action cannot be undone.',
            () => {
                mockBlockedUsers = [];
                renderBlockedList();
                closeModal();
            }
        );
    });

    modalConfirmBtn.addEventListener('click', () => {
        if (confirmCallback) {
            confirmCallback();
        }
    });

    modalCancelBtn.addEventListener('click', closeModal);
    modalOverlay.addEventListener('click', e => {
        if (e.target === modalOverlay) closeModal();
    });
    document.addEventListener('keydown', e => {
        if (e.key === 'Escape' && modalOverlay.classList.contains('active')) {
            closeModal();
        }
    });

    // Initial Render
    renderBlockedList();
});