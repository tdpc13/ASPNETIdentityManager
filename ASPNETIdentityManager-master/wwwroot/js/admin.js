function ajaxCall(url, data) {
    OKButton = document.getElementById("modalOKButton");
    OKButton.disabled = true;
    $.ajax({
        type: "POST",
        url: url,
        //contentType: "application/json; charset=utf-8",
        data: data,
        dataType: "json",
        success: function (response, status) {
            textP = document.createElement("p");
            textP.style.textAlign = "center";
            textP.innerText = response;
            document.getElementById("modal-body").appendChild(textP);
            this.always();
        },
        error: function (error, status) { this.always(); },
        always: function () {
            OKButton = document.getElementById("modalOKButton");
            OKButton.disabled = false;
            OKbutton.onclick = function () {
                location = location;
                //hideModal();
            }
            CancelButton = document.getElementById("modalCancelButton");
            CancelButton.disabled = true;
        }
    });
}

function ajaxCallPrenota(url, data) {
    OKButton = document.getElementById("modalOKButton2");
    OKButton.disabled = true;
    $.ajax({
        type: "POST",
        url: url,
        //contentType: "application/json; charset=utf-8",
        data: data,
        dataType: "json",
        success: function (response, status) {
            textP = document.createElement("p");
            textP.style.textAlign = "center";
            textP.innerText = response;
            document.getElementById("modal-body").appendChild(textP);
            this.always();
        },
        error: function (error, status) { this.always(); },
        always: function () {
            OKButton = document.getElementById("modalOKButton2");
            OKButton.disabled = false;
            OKbutton.onclick = function () {
                location = location;
                //hideModal();
            }
            CancelButton = document.getElementById("modalCancelButton");
            CancelButton.disabled = true;
        }
    });
}

function showInformationModal(text) {
    textP = document.createElement("p");
    textP.style.textAlign = "center";
    textP.innerText = text;
    document.getElementById("modal-body").appendChild(textP);
    $(".modal-footer").empty();
    OKbutton = document.createElement("button");
    OKbutton.innerText = "OK";
    OKbutton.id = "modalOKButton";
    OKbutton.classList.add("btn");
    OKbutton.classList.add("btn-primary");
    OKbutton.onclick = function () {
        location = location;
        //hideModal();
    }
    $(".modal-footer").append(OKbutton);
    document.getElementById("modal").style.display = "block";
}
//<claims>
function showConfirmationModalForClaims(url, data) {
    document.getElementById("modal-header").innerText = data.addOrDelete + " claim ?";
    questionP = document.createElement("p");
    questionP.style.textAlign = "center";
    questionP.innerText = data.addOrDelete + " claim ?";
    document.getElementById("modal-body").appendChild(questionP);

    OKbutton = document.createElement("button");
    OKbutton.innerText = "OK";
    OKbutton.id = "modalOKButton";
    OKbutton.classList.add("btn");
    OKbutton.classList.add("btn-primary");

    if (data.addOrDelete == 'Add') {
        claimTypeP = document.createElement("p");
        claimTypeP.style.textAlign = "center";
        claimTypeP.innerText = 'Claim Type';
        document.getElementById("modal-body").appendChild(claimTypeP);
        claimTypeTextArea = document.createElement("input");
        claimTypeP.appendChild(claimTypeTextArea);

        claimValueP = document.createElement("p");
        claimValueP.style.textAlign = "center";
        claimValueP.innerText = 'Claim Value';
        document.getElementById("modal-body").appendChild(claimValueP);
        claimValueTextArea = document.createElement("input");
        claimValueP.appendChild(claimValueTextArea);

        OKbutton.onclick = function () {
            data.claimtype = claimTypeTextArea.value;
            data.claimvalue = claimValueTextArea.value;
            claimTypeTextArea.disabled = true;
            claimValueTextArea.disabled = true;
            ajaxCall(url, data);
        }
    }
    else {
        OKbutton.onclick = function () {
            ajaxCall(url, data);
        }
    }
    $(".modal-footer").append(OKbutton);
    CancelButton = document.createElement("button");
    CancelButton.innerText = "Cancel";
    CancelButton.id = "modalCancelButton";
    CancelButton.classList.add("btn");
    CancelButton.classList.add("btn-danger");
    CancelButton.onclick = function () {
        hideModal();
    }
    $(".modal-footer").append(CancelButton);
    document.getElementById("modal").style.display = "block";
}
//</claims>

//<roles>
function showConfirmationModalForRoles(url, data) {
    document.getElementById("modal-header").innerText = data.addOrDelete + " role ?";
    questionP = document.createElement("p");
    questionP.style.textAlign = "center";
    questionP.innerText = data.addOrDelete + " role ?";
    document.getElementById("modal-body").appendChild(questionP);

    OKbutton = document.createElement("button");
    OKbutton.innerText = "OK";
    OKbutton.id = "modalOKButton";
    OKbutton.classList.add("btn");
    OKbutton.classList.add("btn-primary");

    if (data.addOrDelete == 'Add') {
        RoleNameP = document.createElement("p");
        RoleNameP.style.textAlign = "center";
        RoleNameP.innerText = 'Role Name';
        document.getElementById("modal-body").appendChild(RoleNameP);
        RoleNameTextArea = document.createElement("input");
        RoleNameP.appendChild(RoleNameTextArea);

        OKbutton.onclick = function () {
            data.roleName = RoleNameTextArea.value;
            RoleNameTextArea.disabled = true;
            ajaxCall(url, data);
        }
    }
    else {
        OKbutton.onclick = function () {
            ajaxCall(url, data);
        }
    }
    $(".modal-footer").append(OKbutton);
    CancelButton = document.createElement("button");
    CancelButton.innerText = "Cancel";
    CancelButton.id = "modalCancelButton";
    CancelButton.classList.add("btn");
    CancelButton.classList.add("btn-danger");
    CancelButton.onclick = function () {
        hideModal();
    }
    $(".modal-footer").append(CancelButton);
    document.getElementById("modal").style.display = "block";
}
//</roles>
function prenota(url, data) {

    document.getElementById("modal-header").innerText = "Aggiungi contatto ?";
    questionP = document.createElement("p");
    questionP.style.textAlign = "center";
    questionP.innerText = "Nuovo contatto";
    document.getElementById("modal-body").appendChild(questionP);

    OKbutton2 = document.createElement("button");
    OKbutton2.innerText = "OK";
    OKbutton2.id = "modalOKButton2";
    OKbutton2.classList.add("btn");
    OKbutton2.classList.add("btn-primary");
    
    dataIniz = document.createElement("p");
    dataIniz.style.textAlign = "center";
    dataIniz.innerText = 'Dal ';
    document.getElementById("modal-body").appendChild(dataIniz);
    dataInizTextArea = document.createElement("input");
    dataInizTextArea.type = "date";
    dataIniz.appendChild(dataInizTextArea);

    dataFine = document.createElement("p");
    dataFine.style.textAlign = "center";
    dataFine.innerText = 'Al ';
    document.getElementById("modal-body").appendChild(dataFine);
    dataFineTextArea = document.createElement("input");
    dataFineTextArea.type = "date";
    dataFine.appendChild(dataFineTextArea);

    quanti = document.createElement("p");
    quanti.style.textAlign = "center";
    quanti.innerText = 'Quanti ';
    document.getElementById("modal-body").appendChild(quanti);
    quantiTextArea = document.createElement("input");
    quanti.appendChild(quantiTextArea);

    OKbutton2.onclick = function () {
        data.dataIniz = dataInizTextArea.value;
        data.dataFine = dataFineTextArea.value;
        data.quanti = quantiTextArea.value;
        dataInizTextArea.disabled = true;
        dataFineTextArea.disabled = true;
        quantiTextArea.disabled = true;
        ajaxCallPrenota(url, data);
    }


    $(".modal-footer").append(OKbutton2);
    CancelButton = document.createElement("button");
    CancelButton.innerText = "Cancel";
    CancelButton.id = "modalCancelButton";
    CancelButton.classList.add("btn");
    CancelButton.classList.add("btn-danger");
    CancelButton.onclick = function () {
        hideModal();
    }
    $(".modal-footer").append(CancelButton);
    document.getElementById("modal").style.display = "block";
}



//<users>
function showConfirmationModalForUsers(url, data) {
    document.getElementById("modal-header").innerText = data.addOrDelete + " user ?";
    questionP = document.createElement("p");
    questionP.style.textAlign = "center";
    questionP.innerText = data.addOrDelete + " user ?";
    document.getElementById("modal-body").appendChild(questionP);

    OKbutton = document.createElement("button");
    OKbutton.innerText = "OK";
    OKbutton.id = "modalOKButton";
    OKbutton.classList.add("btn");
    OKbutton.classList.add("btn-primary");

    if (data.addOrDelete == 'Add' || data.addOrDelete == 'Edit') {
        userNameP = document.createElement("p");
        userNameP.style.textAlign = "center";
        userNameP.innerText = 'Username';
        document.getElementById("modal-body").appendChild(userNameP);
        userNameTextArea = document.createElement("input");
        userNameP.appendChild(userNameTextArea);

        emailP = document.createElement("p");
        emailP.style.textAlign = "center";
        emailP.innerText = 'Email';
        document.getElementById("modal-body").appendChild(emailP);
        emailTextArea = document.createElement("input");
        emailP.appendChild(emailTextArea);

        passwordP = document.createElement("p");
        passwordP.style.textAlign = "center";
        passwordP.innerText = 'Password';
        document.getElementById("modal-body").appendChild(passwordP);
        passwordTextArea = document.createElement("input");
        passwordTextArea.setAttribute("type", "password");
        passwordP.appendChild(passwordTextArea);

        confirmPasswordP = document.createElement("p");
        confirmPasswordP.style.textAlign = "center";
        confirmPasswordP.innerText = 'Confirm Password';
        document.getElementById("modal-body").appendChild(confirmPasswordP);
        confirmPasswordTextArea = document.createElement("input");
        confirmPasswordTextArea.setAttribute("type", "password");
        confirmPasswordP.appendChild(confirmPasswordTextArea);

        OKbutton.onclick = function () {
            data.username = userNameTextArea.value;
            data.email = emailTextArea.value;
            data.password = passwordTextArea.value;
            data.confirmpassword = confirmPasswordTextArea.value;
            userNameTextArea.disabled = true;
            emailTextArea.disabled = true;
            passwordTextArea.disabled = true;
            confirmPasswordTextArea.disabled = true;
            ajaxCall(url, data);
        }
    }
    else {
        OKbutton.onclick = function () {
            ajaxCall(url, data);
        }
    }
    $(".modal-footer").append(OKbutton);
    CancelButton = document.createElement("button");
    CancelButton.innerText = "Cancel";
    CancelButton.id = "modalCancelButton";
    CancelButton.classList.add("btn");
    CancelButton.classList.add("btn-danger");
    CancelButton.onclick = function () {
        hideModal();
    }
    $(".modal-footer").append(CancelButton);
    document.getElementById("modal").style.display = "block";
}
//</users>