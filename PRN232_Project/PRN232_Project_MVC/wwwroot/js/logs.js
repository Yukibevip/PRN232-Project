const tbody = document.querySelector('.timeline-container');
const searchBox = document.querySelector('.search-box');
const filterSelect = document.querySelectorAll('.filter-select');
const excelBtn = document.querySelector('#excelBtn');
//const pagination = document.querySelector('.pagination');
let action, timeFilter, dateNow, dateNowMilisec;


const json = JSON.parse(logs);
//let numPage = Math.ceil(json.length / 5);
let jsonFilter = json;
console.log(jsonFilter);

filterSelect.forEach(item => {
    item.addEventListener('change', () => {
        identifyFilter();

        if (action != "")
            jsonFilter = jsonFilter.filter(item3 => item3.Action.toLowerCase() == action);

        dateNowMilisec = new Date();
        dateNow = new Date(dateNowMilisec.getFullYear(), dateNowMilisec.getMonth(), dateNowMilisec.getDate());
        switch (timeFilter) {
            case "today":
                jsonFilter = jsonFilter.filter(item3 => new Date(item3.TimeStamp) > dateNow);
                break;
            case "yesterday":
                jsonFilter = jsonFilter.filter(item3 => new Date(item3.TimeStamp) > new Date(dateNow.getTime() - 24 * 60 * 60 * 1000));
                break;
            case "week":
                jsonFilter = jsonFilter.filter(item3 => new Date(item3.TimeStamp) > new Date(dateNow.getTime() - 7 * 24 * 60 * 60 * 1000));
                break;
            case "month":
                jsonFilter = jsonFilter.filter(item3 => new Date(item3.TimeStamp) > new Date(dateNow.getTime() - 30 * 24 * 60 * 60 * 1000));
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
            LogId: item.LogId,
            UserId: item.UserId,
            Time: item.TimeStamp,
            ErrorCode: item.ErrorCode,
            Action: item.Action,
            Status: item.Status,
            Description: item.Description
        });
    })

    const worksheet = XLSX.utils.json_to_sheet(excelArray);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1");

    // Xuất file Excel
    XLSX.writeFile(workbook, "logs.xlsx");
});

function loadTbody(list) {
    tbody.innerHTML = '';

    list.forEach(trElement => {
        var item =
            '<div class="log-item message">\n' +
            '<div class="log-header">\n' +
            '<div class="log-user">\n' +
            '<div class="user-avatar">NT</div>\n' +
            '<div class="user-info">\n' +
            '<h4>' + trElement.User.Username + '</h4>\n' +
            '<p>' + trElement.User.Email + '</p>\n' +
            '</div>\n' +
            '</div>\n' +
            '<div class="log-time">5 phút trước</div>\n' +
            '</div>\n' +
            '<div class="log-content">\n' +
            '<div class="log-description">\n' +
            '<span class="badge ' + trElement.Action.toLowerCase() + '">' + trElement.Action + '</span>\n' +
            trElement.Description + '\n' +
            '</div>\n' +
            '<div class="log-meta">\n' +
            '<div class="meta-item">\n' +
            '<span class="meta-icon">🕐</span>\n' +
            '<span>' + trElement.TimeStamp + '</span>\n' +
            '</div>\n' +
            '</div>\n' +
            '</div>\n' +
            '</div>';

        tbody.innerHTML += item;
    });
}

function identifyFilter() {
    action = filterSelect[0].value;
    timeFilter = filterSelect[1].value;
}