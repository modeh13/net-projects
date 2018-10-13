/*
 * File created for controlling the #formOfflineRender.html page.
 */
var loadForm = function() {
  var pos = document.URL.indexOf("?");
  var key = document.URL.substring(pos + 1);
  // Encode key
  key = atob(key);
  updateContent(key)
}

var updateContent = function(key) {
  var viewPortTag = document.createElement('meta');
  viewPortTag.id = "viewport";
  viewPortTag.name = "viewport";
  var auxWidth = parseInt(localStorage.getItem('widthFormValue_' + key));
  viewPortTag.content = "width=" + auxWidth;
  document.getElementsByTagName('head')[0].appendChild(viewPortTag);

  if (document.getElementById("behaviorAffectCompId2")) {
    document.getElementById("behaviorAffectCompId2").value = "";
  }

  var htmlInner = mintDecompress(localStorage.getItem(key));

  var buttonsDiv = "<div id=\"ButtonBar\" style=\"position:relative\">"
      + "<table><tr>"
      + "<td><input type=\"button\" class=\"submitbuttonautowidth\" onclick=\"saveData('"
      + key
      + "');\" value=\"Save Data\" /></td>"
      + "<td style=\"padding-left:10px;\"><input type=\"button\" class=\"submitbuttonautowidth\" onclick=\"returnForm();\" value=\"Return to Form List\" /></td>"
      + "<td style=\"padding-left:10px;\"><input type=\"button\" class=\"submitbuttonautowidth btn_show_select_record_items_class\" onclick=\"return showSelectRecordItemModal();\" value=\"Select Record Items\" /></td>"
      + "</tr></table>"
      + "</div>"
      + "<div id=\"ProgressMessage\" style=\"position:relative; display:none; margin:8px 0px 8px 2px; font-family:Verdana; font-size:8pt; font-weight:bold;\">"
      + "<span>Values are being saved... please wait a moment.</span>"
      + "</div>";

  var errorDiv = "<div id=\"errorComponents\" style=\"position:relative; font-size:9pt; font-weight:bold; margin:10px 1px 1px 1px;\"></div>";

  var contentDiv = "<div class=\"formComponent\" id=\"divContentForm\" style=\"position:relative\">"
      + htmlInner + "</div>";

  var entityBackground = localStorage.getItem('entity_background_' + key);
  var formHeight = localStorage.getItem('entity_height_' + key);
  var rotateForm = localStorage.getItem('rotateForm_' + key);
  var setEntityKeys = localStorage.getItem('entityKeys_' + key);
  var removeSelectRIButtonValue = localStorage.getItem('removeSelectRIButton_' + key);

  if (rotateForm === 'true') {
    rotateForm = true;
  } else {
    rotateForm = false;
  }
  setRotateStandalone(rotateForm);
  
  setRemoveSelectRIButton(removeSelectRIButtonValue === 'true' ? true : false);

  var entityKeysArray = [];

  var splitted = setEntityKeys.split(",");
  for (var i = 0; i < splitted.length; i++) {
    entityKeysArray[i] = splitted[i];
  }

  setSetEntityKeys(entityKeysArray);
  var divMain = document.getElementById("content");
  divMain.innerHTML = buttonsDiv + errorDiv + contentDiv;
  divMain.style.visibility = 'visible';
  divMain.style.display = 'block';

  if (entityBackground != null) {
    document.getElementById('divContentForm').style.backgroundColor = entityBackground;
  }

  if (formHeight != null) {
    document.getElementById('divContentForm').style.height = formHeight;
  }

  // execute script comming in the form div, to declare required objects in
  // offline mode
  var scriptOffline = document.getElementById('scriptOffline');
  if (scriptOffline && scriptOffline.innerHTML) {
    var innHtml = scriptOffline.innerHTML;
    setTimeout(function() {
      eval(innHtml);
    }, 100);
  }

  // set behaviors and mandatory objects
  setStateForm(localStorage.getItem('stateForm_' + key));

  hideCalendarIcons();
  synchronizeFormData(key);
  initProcessToHideAndShowRII();
  hideSelectRIButton();
  hideInvisibleComponents(key);

  // restart orientation
  var lastOrientation = "Portrait";
  moveStandAlone();
}

var hideInvisibleComponents = function(entity) {
  var allKeys = localStorage.getItem("invisible_" + entity);
  if (allKeys) {
    var arrayAllKeys = mintDecompress(allKeys).split(';');
    var sizeKeys = arrayAllKeys.length;
    for (var index = 0; index < sizeKeys; index++) {
      if (document.getElementById(arrayAllKeys[index])) {
        document.getElementById(arrayAllKeys[index]).style.display = 'none';
      }
    }
  }
}

var hideCalendarIcons = function() {
  var calendarIcons = document.getElementsByClassName("inputExtension");
  var numCalendarIcons = calendarIcons.length;
  for (var index = 0; index < numCalendarIcons; index++) {
    if (calendarIcons[index].tagName === "IMG") {
      calendarIcons[index].style.visibility = 'hidden';
    }
  }
}

var getFormBuilderArrays = function(arrayKey, htmlKey) {
  var arrayJsonObject = [];

  var arrayLength = localStorage.getItem(arrayKey + '_' + htmlKey + "_length");
  for (var index = 0; index < arrayLength; index++) {
    arrayJsonObject[index] = JSON.parse(localStorage.getItem(arrayKey + '_'
        + htmlKey + '_' + index));
  }
  return arrayJsonObject;
}

/*
 * Function called when the user save the form.
 */
var saveData = function(entity) {

  // hide last message shown, buttons, and show wait message
  var divElement;
  document.getElementById("errorComponents").innerHTML = "";

  divElement = document.getElementById("ProgressMessage");

  if (divElement !== null && divElement !== undefined) {
    divElement.style.display = 'block';
    divElement.style.visibility = 'visible';
    divElement = document.getElementById("ButtonBar");
    divElement.style.display = 'none';
    divElement.style.visibility = 'hidden';
  }

  // execute the actual save with a delay, so the disable is noticeable
  setTimeout(function() {
    _saveData(entity, true);
  }, 300);
}

/*
 * Internal data save function.
 */
var _saveData = function(entity, fromOffLineMode) {
  var allComponents = [];
  var allFrames = [];
  var textareas = [];
  var comboboxes = [];
  var selectedOptions;
  var index, sizeForm;
  var allKeys = "";
  var msg = "";

  try {
    // first remove all saved data that is replaced, to prevent full errors
    // while saving
    clearLocalStorageComponentsData(entity);

    saveInvisibleComponents(entity);
    // The current style components have to be saved in a localStorage parameter
    // "styleComponents_" + entityKey
    // for the Synchronize Form can load the saved styles for each one
    // component.
    saveStyles(entity, fromOffLineMode);

    if (document.getElementsByTagName) {
      allComponents = document.getElementsByTagName("input");
      textareas = document.getElementsByTagName("textarea");
      comboboxes = document.getElementsByTagName("select");
      allFrames = document.getElementsByTagName("fieldset");
    } else {
      allComponents = document.body.all.tags("input");
      textareas = document.body.all.tags("textarea");
      comboboxes = document.body.all.tags("select");
      allFrames = document.getElementsByTagName("fieldset");
    }

    var componentI;

    sizeForm = allComponents.length;
    for (index = 0; index < sizeForm; index++) {
      componentI = allComponents[index];
      if (componentI.name.indexOf('RECORDITEM') >= 0
          || componentI.name.indexOf('EMPLOYEE') >= 0
          || componentI.name.indexOf('_ENT_') >= 0
          || componentI.name.indexOf('_RI_') >= 0) {
        if (componentI.id.indexOf('CHKB') >= 0
            || componentI.id.indexOf('RADBTN') >= 0) {
          // store only checked elements
          if (componentI.checked) {
            localStorage.setItem(componentI.id, componentI.checked);
          }
        } else {
          // store only filled values, remove empty
          if (componentI.value) {
            localStorage.setItem(componentI.id, componentI.value);
          }
        }
        allKeys += componentI.id + ";";
      }
    }

    sizeForm = textareas.length;
    for (index = 0; index < sizeForm; index++) {
      componentI = textareas[index];
      if (componentI.name.indexOf('RECORDITEM') >= 0
          || componentI.name.indexOf('EMPLOYEE') >= 0
          || componentI.name.indexOf('_ENT_') >= 0
          || componentI.name.indexOf('_RI_') >= 0) {
        // store only filled values, remove empty
        if (componentI.value) {
          localStorage.setItem(componentI.id, componentI.value);
        }
        allKeys += componentI.id + ";";
      }
    }

    sizeForm = comboboxes.length;
    for (index = 0; index < sizeForm; index++) {
      componentI = comboboxes[index];
      if ((componentI.name.indexOf('RECORDITEM') >= 0
          || componentI.name.indexOf('EMPLOYEE') >= 0
          || componentI.name.indexOf('_ENT_') >= 0 || componentI.name
          .indexOf('_RI_') >= 0)) {

        if (componentI.options) {
          selectedOptions = [];
          var sizeOptions = componentI.options.length;
          var indexSelected = 0;
          for (var k = 0; k < sizeOptions; k++) {
            if (componentI.options[k].selected) {
              selectedOptions[indexSelected] = componentI.options[k].value;
              indexSelected++;
            }
          }
          if (indexSelected === 0 && componentI.type === "select-multiple") {
            selectedOptions[0] = "";
            indexSelected++;
          }
          if (indexSelected > 0) {
            localStorage.setItem(componentI.id, selectedOptions);
            allKeys += componentI.id + ";";
          }
        }
      }
    }

    sizeForm = allFrames.length;
    for (index = 0; index < sizeForm; index++) {
      componentI = allFrames[index];
      if (componentI.id.indexOf('RECORDITEM') >= 0
          || componentI.id.indexOf('EMPLOYEE') >= 0
          || componentI.id.indexOf('_ENT_') >= 0
          || componentI.id.indexOf('_RI_') >= 0) {
        allKeys += componentI.id + ";";
      }
    }

    localStorage
        .setItem("filledComponentKeys_" + entity, mintCompress(allKeys));

    msg = "The form values were saved successfully...";
  } catch (e) {
    msg = "The form values were not saved, because the browser cache is full!<br><i>Error details:</i> "
        + e;
  }

  // show result, buttons and hide wait message
  var divElement = document.getElementById("ButtonBar");
  document.getElementById("errorComponents").innerHTML = createInfoBox(msg);

  if (divElement !== null && divElement !== undefined) {
    divElement.style.display = 'block';
    divElement.style.visibility = 'visible';
    divElement = document.getElementById("ProgressMessage");
    divElement.style.display = 'none';
    divElement.style.visibility = 'hidden';
  }

}

/*
 * Save form style.
 */
var saveStyles = function(entity, fromOffLineMode) {
  var styleComp;
  //It is necessary to decide if it is done from online or offline mode.
  if(fromOffLineMode){
    styleComp = document.getElementById("behaviorAffectCompId2");    
  }
  else{
    styleComp = document.getElementById("behaviorAffectCompId");    
  }
  if (styleComp) {
    // do not append old styles because they are already in the comp value
    if (styleComp.value) {
      localStorage.setItem("styleComponents_" + entity,
          mintCompress(styleComp.value));
    } else {
      localStorage.removeItem("styleComponents_" + entity);
    }
  } else {
    localStorage.removeItem("styleComponents_" + entity);
  }
}

/*
 * Save invisible components.
 */
var saveInvisibleComponents = function(entity) {
  var allComponents = document.getElementsByTagName("div");
  var numberComponents = allComponents.length;

  var invisibleDivs = null;
  for (var index = 0; index < numberComponents; index++) {
    var divComp = allComponents[index];
    // skip other divs
    if (divComp.id === 'ButtonBar' || divComp.id === 'ProgressMessage'
        || divComp.id === 'errorComponents') {
      continue;
    }

    if (divComp.style.display === 'none') {
      if (invisibleDivs == null) {
        invisibleDivs = "";
      } else {
        invisibleDivs += ";";
      }
      invisibleDivs += divComp.id;
    }
  }
  if (invisibleDivs != null) {
    localStorage.setItem("invisible_" + entity, mintCompress(invisibleDivs));
  }
}

/*
 * Return to the #formOffline.html page.
 */
var returnForm = function() {
  if (document.getElementById("behaviorAffectCompId2")) {
    document.getElementById("behaviorAffectCompId2").value = "";
  }
  window.location.href = "../../formBuilder/offline/formOffline.html";
}

/*
 * Add some events to the body when the page is loaded.
 */
document.addEventListener("DOMContentLoaded", function() {
  var body = document.getElementsByTagName("body")[0];

  body.addEventListener("load", loadForm());

  body.addEventListener("onorientationchange", moveStandAlone());
});