const tbody = document.querySelector('tbody');
const searchBox = document.querySelector('.search-box');
const filterSelect = document.querySelectorAll('.filter-select');
const pagination = document.querySelector('.pagination');
const excelBtn = document.querySelector('#excelBtn');
let status, category, otherFilter;

const json = JSON.parse(accusations);
let numPage = Math.ceil(json.length / 5);
let jsonFilter = json;

const priorityMap = new Map([
    ['low', 1],
    ['medium', 2],
    ['high', 3]
]);

const accusationsMap = new Map(json.map(a => [a.AccusationId, a]));

const detailModalEl = document.getElementById('accusationDetailModal');
const detailModal = new bootstrap.Modal(detailModalEl);
const confirmModalEl = document.getElementById('confirmActionModal');
const confirmModal = new bootstrap.Modal(confirmModalEl);
const confirmBtn = document.querySelector('#confirmActionModal #confirm-doit')


loadPagination();

pagination.innerHTML = '<button disabled>← Trước</button>' + pagination.innerHTML;
pagination.innerHTML += '<button onclick="showPage(2)">Sau →</button>';


filterSelect.forEach(item => {
    item.addEventListener('change', () => {
        identifyFilter();
        
        if (category != "")
            jsonFilter = jsonFilter.filter(item3 => item3.Category.toLowerCase() == category);

        if (status != "")
            jsonFilter = jsonFilter.filter(item3 => item3.Status.toLowerCase() == status);

        switch (otherFilter) {
            case "newest":
                jsonFilter.sort((a, b) => Date.parse(b.CreatedAt) - Date.parse(a.CreatedAt));
                break;
            case "oldest":
                jsonFilter.sort((a, b) => Date.parse(a.CreatedAt) - Date.parse(b.CreatedAt));
                break;
            case "priority":
                console.log(sortPriority(jsonFilter));
                break;
        }
        console.log(jsonFilter);
        loadTbody(jsonFilter);
        jsonFilter = json;

        document.querySelectorAll('.btn-review').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const tr = e.target.closest('tr');
                const id = parseInt(tr.dataset.id, 10);
                const acc = accusationsMap.get(id);
                if (!acc) return;

                document.getElementById('accusationDetailLabel').textContent = `Chi tiết tố cáo #${id}`;
                document.getElementById('detail-id').textContent = `#${id}`;
                document.getElementById('detail-reported').textContent = `${acc.Reported?.Username ?? acc.Reported?.username ?? ''} (${acc.Reported?.Email ?? acc.Reported?.email ?? ''})`;
                document.getElementById('detail-accused').textContent = `${acc.Accused?.Username ?? acc.Accused?.username ?? ''} (${acc.Accused?.Email ?? acc.Accused?.email ?? ''})`;
                document.getElementById('detail-category').textContent = acc.Category ?? '';
                document.getElementById('detail-descriptions').textContent = acc.Descriptions ?? '';
                document.getElementById('detail-status').textContent = acc.Status ?? '';
                document.getElementById('detail-createdat').textContent = acc.CreatedAt ?? '';
                document.getElementById('detail-resolution').textContent = acc.ResolutionNote ?? '';

                detailModal.show();
            });
        });

        document.querySelectorAll('.btn-resolve').forEach(btn => {
            console.log('hello');
            btn.addEventListener('click', (e) => {
                const tr = e.target.closest('tr');
                const id = parseInt(tr.dataset.id);
                openConfirm(id, 'Resolved');

                confirmBtn.addEventListener('click', () => {
                    fetch("/admin/accusations/resolve/" + id, {
                        method: 'POST'
                    })
                        .then(res => res.json())
                        .then(data => console.log(data));
                });
            });
        });
        document.querySelectorAll('.btn-reject').forEach(btn => {
            console.log('hello');
            btn.addEventListener('click', (e) => {
                const tr = e.target.closest('tr');
                const id = parseInt(tr.dataset.id);
                openConfirm(id, 'Rejected');

                confirmBtn.addEventListener('click', () => {
                    fetch("/admin/accusations/resolve/" + id, {
                        method: 'POST'
                    })
                        .then(res => res.json())
                        .then(data => console.log(data));
                });
            });
        });
    });
});

excelBtn.addEventListener("click", () => {
    let excelArray = new Array();

    json.forEach(item => {
        excelArray.push({
            AccusationId: item.AccusationId,
            AccusedId: item.AccusedId,
            ReportedId: item.ReportedId,
            Time: item.CreatedAt,
            Descriptions: item.Descriptions,
            ReviewAt: item.ReviewAt,
            ReviewedBy: item.ReviewedBy,
            Status: item.Status
        });
    })

    const worksheet = XLSX.utils.json_to_sheet(excelArray);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1");

    // Xuất file Excel
    XLSX.writeFile(workbook, "accusations.xlsx");
});

function loadTbody(list) {
    tbody.innerHTML = '';

    list.forEach(trElement => {
        var item =
            '<tr data-id="' + trElement.AccusationId + '">\n' +
            '<td> <strong>#' + trElement.AccusationId + '</strong></td>\n' +
            '<td>\n' +
            '<div class="user-cell">\n' +
            '<div class="user-avatar">NT</div>\n' +
            '<div class="user-info">\n' +
            '<h4>' + trElement.Reported.Username + '</h4>\n' +
            '<p>' + trElement.Reported.Email + '</p>\n' +
            '</div>\n' +
            '</div>\n' +
            '</td>\n' +
            '<td>\n' +
            '<div class="user-cell">\n' +
            '<div class="user-avatar accused">HQ</div>\n' +
            '<div class="user-info">\n' +
            '<h4>' + trElement.Accused.Username + '</h4>\n' +
            '<p>' + trElement.Accused.Email + '</p>\n' +
            '</div>\n' +
            '</div>\n' +
            '</td>\n' +
            '<td class="reason-cell">\n' +
            '<div class="priority-badge ' + trElement.Category.toLowerCase() + '">' + trElement.Category + '</div>\n' +
            '<div class="reason-text">' + trElement.Descriptions + '</div>\n' +
            '</td>\n' +
            '<td><span class="badge ' + trElement.Status.toLowerCase() + '">' + trElement.Status + '</span></td>\n' +
            '<td>' + formatDate(new Date(trElement.CreatedAt)) + '</td>\n' +
            '<td>\n' +
            '<div class="action-buttons">\n' +
            '<button class="btn-action btn-review btn btn-sm btn-outline-primary">Xem xét</button>\n' +
            '<button class="btn-action btn-resolve btn btn-sm btn-outline-success">Giải quyết</button>\n' +
            '<button class="btn-action btn-reject btn btn-sm btn-outline-danger">Từ chối</button>\n' +
            '</div>\n' +
            '</td>\n' +
            '</tr >';

        tbody.innerHTML += item;
    });
}

function identifyFilter() {
    switch (filterSelect[0].value) {
        case "":
            status = filterSelect[0].value;
            break;
        case "pending":
            status = filterSelect[0].value;
            break;
        case "reviewed":
            status = filterSelect[0].value;
            break;
        case "resolved":
            status = filterSelect[0].value;
            break;
    }

    switch (filterSelect[1].value) {
        case "":
            category = filterSelect[1].value;
            break;
        case "high":
            category = filterSelect[1].value;
            break;
        case "medium":
            category = filterSelect[1].value;
            break;
        case "low":
            category = filterSelect[1].value;
            break;
    }

    switch (filterSelect[2].value) {
        case "":
            otherFilter = filterSelect[2].value;
            break;
        case "newest":
            otherFilter = filterSelect[2].value;
            break;
        case "oldest":
            otherFilter = filterSelect[2].value;
            break;
        case "priority":
            otherFilter = filterSelect[2].value;
            break;
    }
}

function sortPriority(list) {
    for (let i = 0; i < list.length; i++) {
        for (let j = i + 1; j < list.length; j++) {
            let temp = list[i];
            if ((priorityMap.get(list[j].Category.toLowerCase()) - priorityMap.get(list[i].Category.toLowerCase())) > 0) {
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }

    return list;
}

function formatDate(date) {
    let day = String(date.getDate()).padStart(2, "0");
    let month = String(date.getMonth() + 1).padStart(2, "0");
    let year = date.getFullYear();
    let hour = String(date.getHours()).padStart(2, "0");
    let minute = String(date.getMinutes()).padStart(2, "0");

    return `${day}/${month}/${year} ${hour}:${minute}`;
}

function loadPagination() {
    pagination.innerHTML = '';
    for (let i = 1; i <= numPage; i++) {
        pagination.innerHTML += '<button class="active" onclick="showPage(' + i + ')">' + i + '</button>';
    }
}

function paginate(array, page, limit) {
    return array.slice((page - 1) * limit, page * limit);
}

function showPage(page) {
    let items = paginate(jsonFilter, page, 5);
    loadTbody(items);
    loadPagination();
    if (page == 1) {
        pagination.innerHTML = '<button disabled>← Trước</button>' + pagination.innerHTML;
        pagination.innerHTML += '<button onclick="showPage(2)">Sau →</button>';
    } else if (page == numPage) {
        pagination.innerHTML = '<button onclick="showPage(' + Number(page - 1) + ')">← Trước</button>' + pagination.innerHTML;
        pagination.innerHTML += '<button disabled>Sau →</button>';
    } else {
        pagination.innerHTML = '<button onclick="showPage(' + Number(page - 1) + ')">← Trước</button>' + pagination.innerHTML;
        pagination.innerHTML += '<button onclick="showPage(' + Number(page + 1) + ')">Sau →</button>';
    }
}


// wire review buttons
document.querySelectorAll('.btn-review').forEach(btn => {
    btn.addEventListener('click', (e) => {
        const tr = e.target.closest('tr');
        const id = parseInt(tr.dataset.id, 10);
        const acc = accusationsMap.get(id);
        if (!acc) return;

        document.getElementById('accusationDetailLabel').textContent = `Chi tiết tố cáo #${id}`;
        document.getElementById('detail-id').textContent = `#${id}`;
        document.getElementById('detail-reported').textContent = `${acc.Reported?.Username ?? acc.Reported?.username ?? ''} (${acc.Reported?.Email ?? acc.Reported?.email ?? ''})`;
        document.getElementById('detail-accused').textContent = `${acc.Accused?.Username ?? acc.Accused?.username ?? ''} (${acc.Accused?.Email ?? acc.Accused?.email ?? ''})`;
        document.getElementById('detail-category').textContent = acc.Category ?? '';
        document.getElementById('detail-descriptions').textContent = acc.Descriptions ?? '';
        document.getElementById('detail-status').textContent = acc.Status ?? '';
        document.getElementById('detail-createdat').textContent = acc.CreatedAt ?? '';
        document.getElementById('detail-resolution').textContent = acc.ResolutionNote ?? '';

        detailModal.show();
    });
});

// variables for confirm flow
let pendingAction = null;
let pendingId = null;

function openConfirm(id, action) {
    pendingId = id;
    pendingAction = action; // "Resolved" or "Rejected"
    const msgEl = document.getElementById('confirm-message');
    const noteGroup = document.getElementById('optional-note-group');
    document.getElementById('resolution-note').value = '';

    if (action === 'Resolved') {
        msgEl.textContent = `Bạn có chắc chắn muốn GIẢI QUYẾT tố cáo #${id}? Hành động này sẽ đặt trạng thái là "Resolved".`;
        noteGroup.style.display = 'block';
    } else if (action === 'Rejected') {
        msgEl.textContent = `Bạn có chắc chắn muốn TỪ CHỐI tố cáo #${id}? Hành động này sẽ đặt trạng thái là "Rejected".`;
        noteGroup.style.display = 'block';
    } else {
        msgEl.textContent = `Bạn có chắc chắn muốn thực hiện hành động này cho tố cáo #${id}?`;
        noteGroup.style.display = 'none';
    }

    confirmModal.show();
}

document.querySelectorAll('.btn-resolve').forEach(btn => {
    btn.addEventListener('click', (e) => {
        const tr = e.target.closest('tr');
        const id = parseInt(tr.dataset.id);
        openConfirm(id, 'Resolved');

        confirmBtn.addEventListener('click', () => {
            fetch("/admin/accusations/resolve/" + id, {
                method: 'POST'
            })
                .then(res => res.json())
                .then(data => console.log(data));
        });
    });
});
document.querySelectorAll('.btn-reject').forEach(btn => {
    btn.addEventListener('click', (e) => {
        const tr = e.target.closest('tr');
        const id = parseInt(tr.dataset.id);
        openConfirm(id, 'Rejected');

        confirmBtn.addEventListener('click', () => {
            fetch("/admin/accusations/resolve/" + id, {
                method: 'POST'
            })
                .then(res => res.json())
                .then(data => console.log(data));
        });
    });
});