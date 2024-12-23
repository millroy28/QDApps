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

    //console.log("updating InventoryView in cookie to value Stashes");
    document.cookie = "InventoryView=Stashes; path=/";
};

function viewTags(){
    document.getElementById("viewStashesButton").disabled = false;
    document.getElementById("viewTagsButton").disabled = true;
    document.getElementById("viewItemsButton").disabled = false;
    document.getElementById("viewStashes").hidden = true;
    document.getElementById("viewTags").hidden = false;
    document.getElementById("viewItems").hidden = true;

    //console.log("updating InventoryView in cookie to value Tags");
    document.cookie = "InventoryView=Tags; path=/";
    
};

function viewItems(){
    document.getElementById("viewStashesButton").disabled = false;
    document.getElementById("viewTagsButton").disabled = false;
    document.getElementById("viewItemsButton").disabled = true;
    document.getElementById("viewStashes").hidden = true;
    document.getElementById("viewTags").hidden = true;
    document.getElementById("viewItems").hidden = false;

    //console.log("updating InventoryView in cookie to value Items");
    document.cookie = "InventoryView=Items; path=/";
};

function setViewOnLoad(view) {
/*    console.log("Setting view to last known value: " + view);*/
    switch (view) {

        case "${Stashes}":
            viewStashes();
            break;
        case "${Items}":
            viewItems();
            break;
        case "${Tags}":
            viewTags();
            break;
    }
            return;
}

function toggleSubmitButton_Stash() {
    let currentStashId = document.getElementById("currentStashId").value;
    let destinationStashId = document.getElementById("destinationStashId").value;
    let currentStashName = document.getElementById("currentStashName").value;
    let editedStashName = document.getElementById("editedStashName").value;
    let destinationTagId = document.getElementById("addTagId").value;
    let inputs = document.getElementsByTagName("input");

    let isAnythingChecked = false;

    for (let i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "checkbox" && inputs[i].checked == true) {
            isAnythingChecked = true;
        };
    };
    

    if (((destinationStashId != 0 || destinationTagId != 0) && isAnythingChecked)
        || currentStashName != editedStashName) {
        document.getElementById("submitButton").disabled = false;
    } else {
        document.getElementById("submitButton").disabled = true;
    }
    return;
}

function toggleSubmitButton_CreateStash() {
    let stashName = document.getElementById("stashName").value;
    if (stashName.replace(/\s/g, '').length > 0) {
        document.getElementById("submitButton").disabled = false;
    } else {
        document.getElementById("submitButton").disabled = true;
    }
    return;   
}

function toggleSubmitButton_CreateTag() {
    let stashName = document.getElementById("tagName").value;
    if (stashName.replace(/\s/g, '').length > 0) {
        document.getElementById("submitButton").disabled = false;
    } else {
        document.getElementById("submitButton").disabled = true;
    }
    return;
}

function toggleSubmitButton_Tag() {

    let currentTagName = document.getElementById("currentTagName").value;
    let editedTagName = document.getElementById("editedTagName").value;
    let currentTagDescription = document.getElementById("currentTagDescription").value;
    let editedTagDescription = document.getElementById("editedTagDescription").value;
    let currentTagColor = document.getElementById("currentTagColor").value;
    let selectedTagColor = "";

    let availableTagColors = document.getElementsByName("EditedTagColor");
    for (let i = 0; i < availableTagColors.length; i++) {
        if (availableTagColors[i].checked == true) {
            selectedTagColor = availableTagColors[i].value;
        }
    }
    let selectedTagCo = document.getElementById("EditedTagColor").value;


    if (currentTagName != editedTagName
        || currentTagDescription != editedTagDescription
        || selectedTagColor != currentTagColor) {
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


    if ((currentItemName != editedItemName
        || currentStashId != destinationStashId)
        && destinationStashId != 0
        && editedItemName.replace(/\s/g, '').length > 0
        ) {
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

function toggle_modal(hide, elementId) {

    if(hide == true){
        document.getElementById(elementId).style.display = "none";
    }

    if (hide == false) {
        document.getElementById(elementId).style.display = "flex";
    }
    //$('#myModal').on('shown.bs.modal', function () {
    //    $('#myInput').trigger('focus')
    //})

    return;
}
