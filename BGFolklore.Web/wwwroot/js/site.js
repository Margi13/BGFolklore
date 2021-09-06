function jqueryAjaxCall(actionName, eventViewModel, userId, onSuccessFunction) {
    $.ajax({
        type: "POST",
        url: actionName,
        data: eventViewModel,
        ContentType: "application/Json;Charset=utf-8",
        datatype: "Json",
        success: function onSuccess(modalHtml) {
            console.log(eventViewModel);
            onSuccessFunction(modalHtml, eventViewModel, userId);
        },
        error: function onFail(response) {
            alert(response.statusText);
        }
    });
    //$.post({
    //    url:''
    //})
}

function editButtonOnClick(e, modelData, userId) {
    e.preventDefault();
    jqueryAjaxCall('EditEvent', modelData, userId, editEventOnSuccess);
}
function editEventOnSuccess(modalHtml, modelData, userId) {
    console.log('edited');
    
}

function deleteButtonOnClick(e, modelData, userId) {
    e.preventDefault();
    jqueryAjaxCall('DeleteEvent', modelData, userId, deleteEventOnSuccess);
}
function deleteEventOnSuccess(modalHtml, modelData, userId) {
    console.log('deleted');
    location.reload();
}

function createModalOnSuccess(modalHtml, modalData, userId) {
    document.getElementById("modal-container").innerHTML = modalHtml;

    createModalInfo(modalData, userId);
    displayModal();
    closeModal();
}

function createModalInfo(modelData, userId) {

    let eventInfoDiv = document.getElementById('eventInfo');
    eventInfoDiv.innerHTML = '';
    let divsToAppend = createInfoDivs(modelData);
    appendElements(divsToAppend, eventInfoDiv);

    showOwnerOrReaderElements(userId, modelData);

    function createInfoDivs(modelData) {
        return [e('div', {}, modelData.name),
        e('div', {}, modelData.description),
        e('div', {}, 'Телефон за контакт: ' + modelData.phone),
        e('div', {}, 'Адрес: ' + modelData.address)];
    }

    function showOwnerOrReaderElements(userId, modelData) {
        let readerSpan = document.getElementById('readerSpan');
        let readerButtons = document.getElementById('readerButtons');

        readerButtons.children[0].addEventListener('click', e => reportButtonOnClick(e, modelData, userId));

        let ownerSpan = document.getElementById('ownerSpan');
        let ownerButtons = document.getElementById('ownerButtons');

        document.getElementById('editEventBtn').addEventListener('click', e => editButtonOnClick(e, modelData, userId));
        document.getElementById('deleteEventBtn').addEventListener('click', e => deleteButtonOnClick(e, modelData, userId));

        if (userId == modelData.ownerId) {
            readerSpan.style.display = 'none';
            readerButtons.style.display = 'none';

            ownerSpan.style.display = 'block';
            ownerButtons.style.display = 'block';
        } else {
            readerSpan.style.display = 'block';
            readerButtons.style.display = 'block';

            ownerSpan.style.display = 'none';
            ownerButtons.style.display = 'none';
        }
    }
}
function displayModal() {
    // Get the modal
    var modal = $('#myModal')[0];
    modal.style.display = "block";
}
function closeModal() {
    var modal = $('#myModal')[0];
    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];
    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}

function eventRowOnClick(modelData, userId) {
    jqueryAjaxCall('ModalPartial', modelData, userId, createModalOnSuccess);
}


function reportButtonOnClick(e, eventId, ownerId) {
    let reportForm = document.getElementById('reportForm');
    if (reportForm.style.display == 'none') {
        reportForm.style.display = 'flex';
        reportForm.style.flexDirection = "column";
        document.getElementById('eventId').value = eventId;
        document.getElementById('ownerId').value = ownerId;
    } else {
        reportForm.style.display = 'none';
    }
}

function e(type, attributes, ...content) {
    const result = document.createElement(type);

    for (let [attr, value] of Object.entries(attributes || {})) {
        if (attr.substring(0, 2) == 'on') {
            result.addEventListener(attr.substring(2).toLocaleLowerCase(), value);
        } else {
            result[attr] = value;
        }
    }

    content = content.reduce((a, c) => a.concat(Array.isArray(c) ? c : [c]), []);

    content.forEach(e => {
        if (typeof e == 'string' || typeof e == 'number') {
            const node = document.createTextNode(e);
            result.appendChild(node);
        } else {
            result.appendChild(e);
        }
    });

    return result;
}
function appendElements(sourceArray, destinationElement) {
    sourceArray.forEach(element => {
        destinationElement.appendChild(element);
    });
}