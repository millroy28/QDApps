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
