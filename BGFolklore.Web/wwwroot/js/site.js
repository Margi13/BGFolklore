function eventRowOnClick(modelData, userId) {

    addInfoToModal(modelData);

    displayModal();
    showOwnerOrReaderElements(userId, modelData);
    closeModal();

    function addInfoToModal(modelData) {
        appendTextNode('eventName', modelData.name);
        appendTextNode('eventDescription', modelData.description);
        appendTextNode('eventPhone', modelData.phone);
        appendTextNode('eventAddress', modelData.address);
    }

    function showOwnerOrReaderElements(userId, modelData) {
        let readerSpan = document.getElementById('readerSpan');
        let readerButtons = document.getElementById('readerButtons');
        document.getElementById('reportBtn').onclick = (e) => reportButtonOnClick(e, modelData.id, userId);

        if (userId == modelData.ownerId) {
            readerSpan.style.display = 'none';
            readerButtons.style.display = 'none';

            ownerSpan.style.display = 'block';
        } else {
            readerSpan.style.display = 'block';
            readerButtons.style.display = 'block';

            ownerSpan.style.display = 'none';
        }
    }
}

function displayModal() {
    let modal = $('#myModal')[0];
    modal.style.display = 'block';
    $('#reportForm')[0].style.display = 'none';
    document.body.style.overflow = 'hidden';
}

function closeModal() {
    let modal = $('#myModal')[0];
    let span = document.getElementsByClassName('span-close')[0];
    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = 'none';
        document.body.style.overflow = '';
    }
    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = 'none';
            document.body.style.overflow = '';
        }
    }
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
function appendTextNode(elementId, text) {
    let element = document.getElementById(elementId);
    element.textContent = '';
    element.appendChild(document.createTextNode(text));
}