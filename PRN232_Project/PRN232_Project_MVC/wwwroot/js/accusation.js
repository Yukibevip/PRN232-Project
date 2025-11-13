const tbody = document.querySelector('tbody');
const searchBox = document.querySelector('.search-box');
const filterSelect = document.querySelectorAll('.filter-select');
const pagination = document.querySelector('.pagination');
let status, category, otherFilter;

const json = JSON.parse(accusations);
let numPage = Math.ceil(json.length / 5);
let jsonFilter = json;

const priorityMap = new Map([
    ['low', 1],
    ['medium', 2],
    ['high', 3]
]);

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
    });
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
            '<button class="btn-action btn-review">Xem xét</button>\n' +
            '<button class="btn-action btn-resolve">Giải quyết</button>\n' +
            '<button class="btn-action btn-reject">Từ chối</button>\n' +
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
