function showResponce(data) {
    console.log(data);
}

function appendRow(data, resultBlock) {
   
    const promise = new Promise((resolve, reject) => {
        const table = resultBlock;
        table.insertAdjacentHTML('beforeend', data);
        resolve();
    });
    return promise;
};

function getRow(actionUrl) {

    const promise = $.ajax({
        url: actionUrl,
        dataType: 'html'
    });
    return promise; 
}

function generateData(rowsNumber, schemaId, actionUrl, successCallback) {

    $.ajax({
        url: actionUrl,
        data: { id: schemaId, rows: rowsNumber },
        method: 'POST',
        dataType: 'JSON',
        success: function (data) {
            successCallback(data);
        }
    });
};

function editRowData(id, date, idResultBlock, dateResultBlock) {

    const promise = new Promise((resolve, reject) => {
        const spanId = document.querySelector(idResultBlock);
        spanId.textContent = id;
        spanId.id += `-${id}`;
        const spanDate = document.querySelector(dateResultBlock);
        spanDate.textContent = date;
        spanDate.id += `-${id}`;
        resolve();
    });
    return promise; 
}

function setReadyState(name, linkId, actionUrl, badgeId) {
    
    const link = document.querySelector(linkId);
    if (link.classList.contains('disabled')) {
        link.classList.remove('disabled');
    }
    link.href = actionUrl + '/name=' + name;
    link.id = '';
    
    const badge = document.querySelector(badgeId);
    badge.classList.replace('bg-secondary', 'bg-success');
    badge.textContent = 'Ready';
    badge.id = '';
}

