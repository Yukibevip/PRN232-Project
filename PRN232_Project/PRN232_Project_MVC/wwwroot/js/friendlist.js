const tbody = document.querySelector('tbody');
const filterSelect = document.querySelectorAll('.filter-select');
const pagination = document.querySelector('.pagination');
const excelBtn = document.querySelector('#excelBtn');

let status, otherFilter;

const json1 = JSON.parse(friendlists);
const json2 = JSON.parse(friendinvitations);
const json4 = JSON.parse(messages);
let json = [];
console.log(json1);
console.log(json2);
console.log(json4);

json = json.concat(json1.map(item => ({
    UserId1: item.UserId1,
    UserId2: item.UserId2,
    Status: "accepted",
    CreatedAt: item.CreatedAt,
    User1: item.UserId1Navigation,
    User2: item.UserId2Navigation
})));

json = json.concat(json2.map(item => ({
    UserId1: item.SenderId,
    UserId2: item.ReceiverId,
    Status: "pending",
    CreatedAt: item.SentAt,
    User1: item.Sender,
    User2: item.Receiver
})));

let jsonFilter = json;
console.log(jsonFilter);


////let numPage = Math.ceil(json.length / 5);


filterSelect.forEach(item => {
    item.addEventListener('change', () => {
        identifyFilter();

        if (status != "")
            jsonFilter = jsonFilter.filter(item3 => item3.Status.toLowerCase() == status);

        switch (otherFilter) {
            case "newest":
                jsonFilter.sort((a, b) => Date.parse(b.CreatedAt) - Date.parse(a.CreatedAt));
                break;
            case "oldest":
                jsonFilter.sort((a, b) => Date.parse(a.CreatedAt) - Date.parse(b.CreatedAt));
                break;
        }
        console.log(jsonFilter);
        loadTbody(jsonFilter);
        jsonFilter = json;
    });
});

excelBtn.addEventListener("click", () => {
    let excelArray = new Array();

    json.forEach(item => {
        excelArray.push({
            UserId1: item.UserId1,
            UserId2: item.UserId2,
            Status: item.Status,
            CreatedAt: item.CreatedAt
        });
    })

    const worksheet = XLSX.utils.json_to_sheet(excelArray);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1");

    // Xuất file Excel
    XLSX.writeFile(workbook, "friendlists.xlsx");
});

function loadTbody(list) {
    tbody.innerHTML = '';

    list.forEach(trElement => {
        var numMessages = Array.from(json4).filter(m => (m.SenderId === trElement.UserId1 && m.ReceiverId === trElement.UserId2) || (m.SenderId === trElement.UserId2 && m.ReceiverId === trElement.UserId1)).length;
        var item =
            '<tr data-user1="' + trElement.UserId1 + '" data-user2="' + trElement.UserId2 + '" data-status="' + trElement.Status + '">' +
            '<td>' +
            '<div class="user-cell">' +
            '<div class="user-avatar">NT</div>' +
            '<div class="user-info">' +
            '<h4>' + trElement.User1.Username + '</h4>' +
            '<p>' + trElement.User1.Email + '</p>' +
            '</div>' +
            '</div>' +
            '</td>' +
            '<td>' +
            '<div class="connection-arrow">⟷</div>' +
            '</td>' +
            '<td>' +
            '<div class="user-cell">' +
            '<div class="user-avatar">LA</div>' +
            '<div class="user-info">' +
            '<h4>' + trElement.User2.Username + '</h4>' +
            '<p>' + trElement.User2.Email + '</p>' +
            '</div>' +
            '</div>' +
            '</td>' +
            '<td><span class="badge ' + trElement.Status + '">' + trElement.Status + '</span></td>' +
            '<td>' + trElement.CreatedAt + '</td>' +
            '<td>' + numMessages + '</td>' +
            '<td>' +
            '<div class="action-buttons">' +
            '<button class="btn-action btn-view btn btn-sm btn-outline-primary">Chi tiết</button>' +
            '<button class="btn-action btn-remove btn btn-sm btn-outline-danger">Xóa</button>' +
            '</div>' +
            '</td>' +
            '</tr>';

        tbody.innerHTML += item;
    });
}

// helpers
function identifyFilter() {
    status = filterSelect[0].value;
    otherFilter = filterSelect[1].value;
}

function formatDate(date) {
    let d = new Date(date);
    let day = String(d.getDate()).padStart(2, "0");
    let month = String(d.getMonth() + 1).padStart(2, "0");
    let year = d.getFullYear();
    let hour = String(d.getHours()).padStart(2, "0");
    let minute = String(d.getMinutes()).padStart(2, "0");
    return `${day}/${month}/${year} ${hour}:${minute}`;
}

// modals & confirm flow
const detailModalEl = document.getElementById('friendDetailModal');
const detailModal = new bootstrap.Modal(detailModalEl);
const confirmModalEl = document.getElementById('confirmFriendModal');
const confirmModal = new bootstrap.Modal(confirmModalEl);
const confirmBtn = document.getElementById('confirm-doit');
let pendingAction = null;
let pendingUser1 = null;
let pendingUser2 = null;
let pendingStatus = null;

// read anti-forgery token inserted by Razor
function getCsrfToken() {
    const el = document.querySelector('input[name="__RequestVerificationToken"]');
    return el ? el.value : '';
}

// endpoints (injected by Razor in the view)
//const blockUrl = typeof blockFriendUrl !== 'undefined' ? blockFriendUrl : '/admin/blockfriend';
//const removeUrl = typeof removeFriendUrl !== 'undefined' ? removeFriendUrl : '/admin/removefriend';

// event delegation for buttons (works after loadTbody)
document.addEventListener('click', (e) => {
    // DETAIL
    if (e.target.matches('.btn-view')) {
        const tr = e.target.closest('tr');
        if (!tr) return;
        const user1Id = tr.dataset.user1;
        const user2Id = tr.dataset.user2;
        const status = tr.dataset.status;

        // find item in json by pair
        const pair = json.find(p => String(p.UserId1) === String(user1Id) && String(p.UserId2) === String(user2Id) && String(p.Status) === String(status));
        if (!pair) return;

        document.getElementById('friendDetailLabel').textContent = `Chi tiết kết nối`;
        document.getElementById('detail-user1').textContent = `${pair.User1?.Username ?? ''} (${pair.User1?.Email ?? ''})`;
        document.getElementById('detail-user2').textContent = `${pair.User2?.Username ?? ''} (${pair.User2?.Email ?? ''})`;
        document.getElementById('detail-status').textContent = pair.Status ?? '';
        document.getElementById('detail-date').textContent = pair.CreatedAt ?? '';
        const numMessages = Array.from(json4).filter(m => (m.SenderId === pair.UserId1 && m.ReceiverId === pair.UserId2) || (m.SenderId === pair.UserId2 && m.ReceiverId === pair.UserId1)).length;
        document.getElementById('detail-messages').textContent = numMessages;

        detailModal.show();
        return;
    }

    // REMOVE
    if (e.target.matches('.btn-remove')) {
        const tr = e.target.closest('tr');
        if (!tr) return;
        pendingUser1 = tr.dataset.user1;
        pendingUser2 = tr.dataset.user2;
        pendingStatus = tr.dataset.status;
        pendingAction = 'remove';
        document.getElementById('confirm-friend-message').textContent = `Bạn có chắc chắn muốn XÓA mối liên hệ giữa User ${pendingUser1} và ${pendingUser2}?`;
        confirmModal.show();
        return;
    }
});

// single handler for confirm button (prevents multiple listeners)
confirmBtn.addEventListener('click', async () => {
    if (!pendingAction || !pendingUser1 || !pendingUser2) return;
    const token = getCsrfToken();

    if (pendingAction === 'remove' && pendingStatus == 'accepted') {
        // optimistic UI: remove row
        const row = document.querySelector(`tr[data-user1="${pendingUser1}"][data-user2="${pendingUser2}"]`);
        const removedRowHtml = row ? row.outerHTML : null;
        if (row) row.remove();

        try {
            const res = await fetch('/admin/friendlist/remove?user1Id=' + pendingUser1 + '&user2Id=' + pendingUser2, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ userId1: pendingUser1, userId2: pendingUser2 })
            });
            if (!res.ok) throw new Error('Server error');
        } catch (err) {
            alert('Xóa thất bại, trang sẽ được tải lại để đồng bộ.');
            location.reload();
        } finally {
            pendingStatus = pendingAction = pendingUser1 = pendingUser2 = null;
            confirmModal.hide();
        }
    }

    if (pendingAction === 'remove' && pendingStatus == 'pending') {
        // optimistic UI: remove row
        const row = document.querySelector(`tr[data-user1="${pendingUser1}"][data-user2="${pendingUser2}"]`);
        const removedRowHtml = row ? row.outerHTML : null;
        if (row) row.remove();

        try {
            const res = await fetch('/admin/friendinvitation/remove?user1Id=' + pendingUser1 + '&user2Id=' + pendingUser2, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ userId1: pendingUser1, userId2: pendingUser2 })
            });
            if (!res.ok) throw new Error('Server error');
        } catch (err) {
            alert('Xóa thất bại, trang sẽ được tải lại để đồng bộ.');
            location.reload();
        } finally {
            pendingStatus = pendingAction = pendingUser1 = pendingUser2 = null;
            confirmModal.hide();
        }
    }
});

// initial render
loadTbody(json);