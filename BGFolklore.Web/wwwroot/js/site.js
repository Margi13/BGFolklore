function eventRowOnClick(modelData, userId) {
    let eventInfoDiv = document.getElementById('eventInfo');
    eventInfoDiv.innerHTML = '';
    let divsToAppend = createInfoDivs(modelData);
    appendElements(divsToAppend, eventInfoDiv);

    displayModal();
    showOwnerOrReaderElements(userId, modelData);
    closeModal();

    function createInfoDivs(modelData) {
        return [e('div', {}, modelData.name),
        e('div', {}, modelData.description),
        e('div', {}, 'Телефон за контакт: ' + modelData.phone),
        e('div', {}, 'Адрес: ' + modelData.address)];
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
    function showOwnerOrReaderElements(userId, modelData) {
        let readerSpan = document.getElementById('readerSpan');
        let readerButton = document.getElementById('readerButton');

        let ownerSpan = document.getElementById('ownerSpan');
        let ownerButtons = document.getElementById('ownerButtons');
        if (userId == modelData.ownerId) {
            readerSpan.style.display = 'none';
            readerButton.style.display = 'none';

            ownerSpan.style.display = 'block';
            ownerButtons.style.display = 'block';
        } else {
            readerSpan.style.display = 'block';
            readerButton.style.display = 'block';

            ownerSpan.style.display = 'none';
            ownerButtons.style.display = 'none';
        }
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

//function createPager(page, pages, header) {
//    const type = header ? 'header' : 'footer';
//    const result = e(type, { className: 'section-title' }, `Page ${page} of ${pages}`);
//    if (page > 1) {
//        result.appendChild(e('a', {
//            href: '/catalog',
//            className: 'pager',
//            onClick: (e) => {
//                e.preventDefault();
//                nav.goTo('catalog', page - 1);
//            }
//        }, '< Prev'));
//    }
//    if (page < pages) {
//        result.appendChild(e('a', {
//            href: '/catalog',
//            className: 'pager',
//            onClick: (e) => {
//                e.preventDefault();
//                nav.goTo('catalog', page + 1);
//            }
//        }, 'Next >'));
//    }
//    return result;
//}
