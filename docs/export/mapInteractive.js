function loadMap(idMap) {
  var objMap = document.getElementById(idMap);
  window.alert("Loading map interactive for " + idMap);
  
  var map    = objMap.contentDocument.querySelector("svg");
  console.log(map);
  
  var toolTip = document.getElementById("toolTip" + idMap);

  // Add event listeners to map element
  if (!/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
    // If user agent is not mobile add click listener (for wikidata links)
    map.addEventListener("click", handleClick, false);
  }
  map.addEventListener("mousemove", mouseEntered, false);
  map.addEventListener("mouseout", mouseGone, false);
}
  // Show tooltip on mousemove
  function mouseEntered(e) {
    var target = e.target;
    if (target.nodeName == "path") {
      target.style.opacity = 0.6;
      var details = e.target.attributes;

      // Follow cursor
      toolTip.style.transform = `translate(${e.offsetX}px, ${e.offsetY}px)`;

      // Tooltip data
      toolTip.innerHTML = `
        <ul>
            <li><b>Province: ${details.name.value}</b></li>
            <li>Local name: ${details.gn_name.value}</li>
            <li>Country: ${details.admin.value}</li>
            <li>Postal: ${details.postal.value}</li>
        </ul>`;
    }
  }