function reveal(id) {
    var f = document.getElementById(id);
    var g = document.getElementById("return");
    f.style.display = 'table-cell';
    g.style.display = 'none';
}

function emptyText() {
    var n = document.getElementById("departureCity");
    var s = document.getElementById("destinationCity");
    var t = document.getElementById("adults");
    var u = document.getElementById("dp_a");
    if (n.value == "") {
        n.style.backgroundColor = "yellow";
        alert("Fill the departure city field!!!");
    }
    if (s.value == "") {
        s.style.backgroundColor = "yellow";
        alert("Fill the destination city field!!!");
    }
    if (t.value == "") {
        t.style.backgroundColor = "yellow";
        alert("Fill the number of persons!!!");
    }
    if (u.value == "") {
        u.style.backgroundColor = "yellow";
        alert("Fill the departure date field!!!");
    }
}

function clearBox() {
    var n = document.getElementById("departureCity");
    var s = document.getElementById("destinationCity");
    var t = document.getElementById("adults");
    var u = document.getElementById("dp_a");
    n.style.backgroundColor = "white";
    s.style.backgroundColor = "white";
    t.style.backgroundColor = "white";
    u.style.backgroundColor = "white";
}


