function deletePopup(url, title) {
    $.ajax({
        method: 'GET',
        url: url,
        success: function(res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
        }
    });
}

function getColumns(form, url) {
    var promise = $.ajax({
        url: url,
        data: new FormData(form),
        contentType: false,
        processData: false,
        method: 'POST',
        dataType: 'html'
    });
    return promise;
}

function appendRow(data, resultBlock, buttonToDisable) {

    const promise = new Promise((resolve, reject) => {
        const table = resultBlock;
        table.insertAdjacentHTML('beforeend', data);
        buttonToDisable.classList.add('disabled');
        resolve();
    });
    return promise;
};

function getRow(actionUrl, tableRowQuantity) {

    const promise = $.ajax({
        url: actionUrl,
        data: { number: tableRowQuantity },
        dataType: 'html'
    });
    return promise;
}

function generateData(rowsNumber, schemaId, actionUrl, successCallback) {

    $.ajax({
        url: actionUrl,
        data: { id: schemaId, rows: rowsNumber },
        method: 'POST',
        dataType: 'TEXT',
        success: function (data) {
            successCallback(data);
        }
    });
};

function setReadyState(name, actionUrl, buttonToEnable, rowId) {

    const row = document.querySelector(rowId);
    const link = row.querySelector('a');

    link.href = actionUrl + '/name=' + name;
    if (link.classList.contains('disabled')) {
        link.classList.remove('disabled');
    }

    const badge = row.querySelector('span');
    badge.classList.replace('bg-secondary', 'bg-success');
    badge.textContent = 'Ready';

    const rowNumber = row.querySelector('th').textContent;
    row.id += '-' + rowNumber;

    if (buttonToEnable.classList.contains('disabled')) {
        buttonToEnable.classList.remove('disabled');
    }
}

