const tbody = document.querySelector('tbody');
const filterSelect = document.querySelectorAll('.filter-select');
let status, category, otherFilter;

let json = JSON.parse(accusations);

console.log(json);

//switch (filterSelect[0].value) {
//    case "":
//        status = filterSelect[0].value;
//        break;
//    case "pending":
//        status = filterSelect[0].value;
//        break;
//    case "reviewed":
//        status = filterSelect[0].value;
//        break;
//    case "resolved":
//        status = filterSelect[0].value;
//        break;
//}

//switch (filterSelect[1].value) {
//    case "":
//        category = filterSelect[1].value;
//        break;
//    case "high":
//        category = filterSelect[1].value;
//        break;
//    case "medium":
//        category = filterSelect[1].value;
//        break;
//    case "low":
//        category = filterSelect[1].value;
//        break;
//}

//switch (filterSelect[2].value) {
//    case "":
//        otherFilter = filterSelect[2].value;
//        break;
//    case "newest":
//        otherFilter = filterSelect[2].value;
//        break;
//    case "oldest":
//        otherFilter = filterSelect[2].value;
//        break;
//    case "priority":
//        otherFilter = filterSelect[2].value;
//        break;
//}

tbody.childNodes.foreach(item => {
    item.style.display = "block";
});

tbody.childNodes.forEach(item => {
    if (category != "" && status != "") {
        if (item.childNodes[3].firstChild.textContent.toLowerCase() != category ||
            item.childNodes[4].firstChild.textContent.toLowerCase() != status) {
            item.style.display = "none";
        }
    }
});

tbody.innerHTML = '';
function createTrElement(trElement) {
    var trElement =
        '<tr data-id="' + trElement.AccusationId + '">\n' +
        '< td > <strong>#' + trElement.AccusationId + '</strong></td>\n' +
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
        '<div class="priority-badge ' + trElement.Category.toLowerCase + '">' + trElement.Category + '</div>\n' +
        '<div class="reason-text">' + trElement.Descriptions + '</div>\n' +
        '</td>\n' +
        '<td><span class="badge ' + trElement.Status.toLowerCase + '">' + trElement.Status + '</span></td>\n' +
        '<td>' + trElement.CreatedAt + '</td>\n' +
        '<td>\n' +
        '<div class="action-buttons">\n' +
        '<button class="btn-action btn-review">Xem xét</button>\n' +
        '<button class="btn-action btn-resolve">Giải quyết</button>\n' +
        '<button class="btn-action btn-reject">Từ chối</button>\n' +
        '</div>\n' +
        '</td>\n' +
        '</tr >';
}
 