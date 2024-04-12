
function switchFormFocus(signUp, signIn) {
    let signUpWidth = signUp.getBoundingClientRect().width;
    let signInWidth = signIn.getBoundingClientRect().width;
    if (signInWidth > signUpWidth) {
        signUp.style.transform = 'translateX(-' + signInWidth + 'px)';
        signUp.children[0].style.transition = 'opacity 0.1s ease-in-out';
        signIn.children[0].style.transition = 'opacity 0.1s ease-in-out';
        signUp.children[0].style.opacity = '0';
        signIn.children[0].style.opacity = '0';
    }
    else if (signUpWidth > signInWidth) {
        signIn.style.transform = 'translateX(' + signUpWidth + 'px)';
        signUp.children[0].style.transition = 'opacity 0.1s ease-in-out';
        signIn.children[0].style.transition = 'opacity 0.1s ease-in-out';
        signIn.children[0].style.opacity = '0';
        signUp.children[0].style.opacity = '0';
    }
}

function opacityToggle(signUp, signIn) {
    signUp.children[0].style.transition = 'opacity 0.1s ease-in-out';
    signIn.children[0].style.transition = 'opacity 0.1s ease-in-out';
    signUp.children[0].style.opacity = '1';
    signIn.children[0].style.opacity = '1';
}

function listItemToggle(list, listNum) {
    let listArray = Array.from(list.children);
    console.log(listArray);
    for (let i = 0; i < listArray.length; i++) {
        if (i == listNum) {
            listArray[i].classList.add("list-toggle");
        }
        else {
            listArray[i].classList.remove("list-toggle");
        }
    }
}

function numberListItemToggle(list, listNum) {
    let listArray = Array.from(list.children);
    listArray.pop();
    listArray.shift();
    for (let i = 0; i < listArray.length; i++) {
        if (i == listNum) {
            listArray[i].classList.add("list-toggle");
        }
        else {
            listArray[i].classList.remove("list-toggle");
        }
    }
}

function initializePopovers(listItem) {
    let listArray = Array.from(listItem.children);
    listArray.forEach(e => {
        $(function () {
            $(e.children[0]).popover()
        })
    })
    
}

function refreshPage() {
    setTimeout("location.reload(true);", timeout);
}

function getListNumber(list) {
    let listArray = Array.from(list.children);
    return listArray.findIndex(e => e.classList.contains('list-toggle'))
}

// Face Detection

function borderFace(imageContainer, rectIndex, width, height, left, top) {
    let childrenArray = Array.from(imageContainer.children);
    let image = childrenArray.shift();
    let rectangle = childrenArray[rectIndex];
    let widthPerc = width / image.naturalWidth * 100;
    let heightPerc = height / image.naturalHeight * 100;

    let leftPerc = left / image.naturalWidth * 100;
    let topPerc = top / image.naturalHeight * 100;

    rectangle.style.top = topPerc + '%';
    rectangle.style.height = heightPerc + '%';

    rectangle.style.left = leftPerc + '%';
    rectangle.style.width = widthPerc + '%';
}

function selectFace(imageContainer, rectIndex) {
    let childrenArray = Array.from(imageContainer.children);
    childrenArray.shift();
    for (let i = 0; i < childrenArray.length; i++) {
        childrenArray[i].style.border = "3px solid red";
        if (i == rectIndex) {
            childrenArray[i].style.border = "3px solid yellow";
        }
    }
}