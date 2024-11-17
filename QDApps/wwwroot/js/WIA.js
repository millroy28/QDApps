function setDefaults(defaults) {
    if (defaults == "stashes") {
        document.getElementById("viewStashesButton").disabled = true;
        document.getElementById("viewStashes").hidden = false;
    }
    if (defaults == "items") {
        document.getElementById("viewItemsButton").disabled = true;
        document.getElementById("viewItems").hidden = false;
    }
    if (defaults == "tags") {
        document.getElementById("viewTagsButton").disabled = true;
        document.getElementById("viewTags").hidden = false;
    }
    return;
}


function viewStashes(){
    document.getElementById("viewStashesButton").disabled = true;
    document.getElementById("viewTagsButton").disabled = false;
    document.getElementById("viewItemsButton").disabled = false;
    document.getElementById("viewStashes").hidden = false;
    document.getElementById("viewTags").hidden = true;
    document.getElementById("viewItems").hidden = true;
};

function viewTags(){
    document.getElementById("viewStashesButton").disabled = false;
    document.getElementById("viewTagsButton").disabled = true;
    document.getElementById("viewItemsButton").disabled = false;
    document.getElementById("viewStashes").hidden = true;
    document.getElementById("viewTags").hidden = false;
    document.getElementById("viewItems").hidden = true;
};

function viewItems(){
    document.getElementById("viewStashesButton").disabled = false;
    document.getElementById("viewTagsButton").disabled = false;
    document.getElementById("viewItemsButton").disabled = true;
    document.getElementById("viewStashes").hidden = true;
    document.getElementById("viewTags").hidden = true;
    document.getElementById("viewItems").hidden = false;
};

function toggleSubmitButton_Stash() {
    let currentStashId = document.getElementById("currentStashId").value;
    let destinationStashId = document.getElementById("destinationStashId").value;
    let currentStashName = document.getElementById("currentStashName").value;
    let editedStashName = document.getElementById("editedStashName").value;
    let inputs = document.getElementsByTagName("input");

    let isAnythingChecked = false;

    for (let i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "checkbox" && inputs[i].checked == true) {
            isAnythingChecked = true;
        };
    };
    

    if (((destinationStashId != 0) && isAnythingChecked)
        || currentStashName != editedStashName) {
        document.getElementById("submitButton").disabled = false;
    } else {
        document.getElementById("submitButton").disabled = true;
    }
    return;
}

function toggleSubmitButton_Tag() {

    let currentTagName = document.getElementById("currentTagName").value;
    let editedTagName = document.getElementById("editedTagName").value;


    if (currentTagName != editedTagName) {
        document.getElementById("submitButton").disabled = false;
    } else {
        document.getElementById("submitButton").disabled = true;
    }
    return;
}

function toggleSubmitButton_Item() {

    let currentItemName = document.getElementById("itemName").value;
    let editedItemName = document.getElementById("editedItemName").value;
    let currentStashId = document.getElementById("stashId").value;
    let destinationStashId = document.getElementById("destinationStashId").value;

    if (currentItemName != editedItemName
        || currentStashId != destinationStashId) {
        document.getElementById("submitButton").disabled = false;
    } else {
        document.getElementById("submitButton").disabled = true;
    }
    return;
}

function toggle_select_all_text_headers(selectAllId) {
    let inputs = document.getElementsByTagName("input");
    let selectAll = document.getElementById(selectAllId);

    for (let i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "checkbox") {
            inputs[i].checked = selectAll.checked;
        };
    };
    return;
}