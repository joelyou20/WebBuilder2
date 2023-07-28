function expandScript(index) {
    let card = document.getElementById(`script-${index}`);
    if (card.classList.contains("expanded")) {
        card.classList.remove("expanded")
    }
    else {
        card.classList.add("expanded")
    }
}