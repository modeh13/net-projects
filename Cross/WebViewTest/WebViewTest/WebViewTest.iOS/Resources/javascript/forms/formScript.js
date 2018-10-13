var entityMap;
var recordItemsMap;
var componentsNamesMap;
var radioButtonNamesMap;
var behaviorComponentNames;
var setEntityKeys;
var componentsErrorMap;
var compChexkBoxYesNoMap;
var _FG_tracker = null;
var rotateStandAlone = false;
var arrayPositions = {};
var reducedSize = 0;
var arrayIterators = [];
var arrayHorizontalPositions = {};
var typeBehaviors = [];
var sequenceMinimumValToContainerMap = [];
var sequenceRealValToContainerMap = [];
var sequenceComponentValueMap;
var currentRadioSelectedByGroupMap = [];
var invalidRefToProp;
var invalidRefToMandNoDefValProp;
var invalidRefsSignOff;
var removeSelectRIButton;
var dateFormat;
var javaDateFormat;
var dateTimeFormat;
var HTML_LABEL_ID = "FB_LABEL";
var HTML_LINE_ID = "FB_LINE";
var HTML_TABLE_ID = "FB_TABLE";
var HTML_LINK_ID = "FB_LINK";
var HTML_IMAGE_ID = "FB_IMAGE";
var HTML_FRAME_ID = "FB_FRAME";
var HTML_LINK_POPUP_ID = "FB_LINKPOP";
var finishTimeOut;
var lastOrientation = "portrait";
var rotateTimeout;
var thereIsError = false;
var mandatory_Icon;
var maxWidthForm;
var calendars = Array();
var calendarsIndex = 0;
var stateForm = null;

function submitFormAction() {
  var canContinue = false;
  hideButtonsShowProgressMessages("Submit");
  var arrayVisibleERI = getSelectedERIKeys(true);
  var arrayNotVisibleERI = getSelectedERIKeys(false);
  try {
    fillMinimumValSequenceToContainer(sequenceMinimumValToContainerMap,
        "_minValSeq:", "sequenceMinValToContainer");
    if (validateDatesFormat(arrayVisibleERI, arrayNotVisibleERI)) {
      fillPropHtml(false);
      canContinue = validateMandatories(arrayVisibleERI, arrayNotVisibleERI)
          && validateTypeBehaviors();
    }
  } catch (err) {
    // ignore
  }
  if (!canContinue) {
    showButtonsHideProgressMessages();
  } else {
    removeDisabledToReadOnlyComponents();
  }
  return canContinue;
}

function getSelectedERIKeys(visible) {

  var arraySelectedERI = [];

  var hiddenSelectedInputs = jQuery('input:hidden[id*=selectable_record_items_]');
  if (hiddenSelectedInputs.length > 0) {
    var firstSelectedInput = hiddenSelectedInputs[0];
    if (firstSelectedInput.value) {
      var eris = JSON.parse(firstSelectedInput.value);
      var index = 0;
      for (var i = 0; i < eris.event_record_items.length; i++) {
        var eri = eris.event_record_items[i];
        if (visible && eri.selected || (!visible && !eri.selected)) {
          arraySelectedERI[index++] = eri.event_record_item_key;
        }
      }
    }
  }
  return arraySelectedERI;
}

function saveFormAction() {
  var canContinue = false;
  hideButtonsShowProgressMessages("Save");

  try {
    if (validateTypeBehaviors()) {
      fillPropHtml(false);
      canContinue = true;
    }
  } catch (err) {
    // ignore
  }
  if (!canContinue) {
    showButtonsHideProgressMessages();
  } else {
    removeDisabledToReadOnlyComponents();
  }
  return canContinue;
}

function modifyFormAction() {
  hideButtonsShowProgressMessages(null);
  fillMinimumValSequenceToContainer(sequenceRealValToContainerMap,
      "_minValSeq:", "sequenceRealValToContainer");
}

function mintDecompress(compressedInfo) {
  if (isIExplorer()) {
    return LZString.decompressFromUTF16(compressedInfo);
  }
  return LZString.decompress(compressedInfo);
}

function mintCompress(info) {
  if (isIExplorer()) {
    return LZString.compressToUTF16(info);
  }
  return LZString.compress(info);
}

function isIExplorer() {
  var NAV = navigator.appName;
  if (NAV === "Microsoft Internet Explorer" || NAV === "Netscape") {
    return true;
  }
  return false;
}

function escapeSeparators(strValue) {
  if (strValue) {
    strValue = strValue.replace(/-,-/g, "-&#44;-").replace(/-;-/g, "-&#59;-");
  }
  return strValue;
}

function removeCssSpaces(strStyle) {
  if (strStyle) {
    strStyle = strStyle.replace(/: /g, ":").replace(/; /g, ";");
  }
  return strStyle;
}

function replaceTop(strId, strStyle, skeepReplaceTop) {
  if (!skeepReplaceTop) {
    var allElements = getAllMovedElements();
    var top = getTopMovedObject(allElements, strId);

    if (top && top != "NotFound" && strStyle && strStyle.indexOf("top") >= 0) {
      var styleTokens = strStyle.split(";");
      var token;
      strStyle = "";
      for (var i = 0; i < styleTokens.length; i++) {
        token = styleTokens[i].trim();
        if (token.indexOf("top") == 0)
          strStyle += "top:" + top + "px;";
        else
          strStyle += token + ";";
      }
    }
  }
  return strStyle;
}

function fillPropHtml(skeepReplaceTop) {
  var divParent = document.getElementsByClassName("renderContainer")[0];
  var elementsArrayLabel = divParent.getElementsByTagName('div');

  var stylesElem = "";
  var containerStyles = "";
  var mandatoryElem = "";
  var disabledElem = "";
  var readOnlyElem = "";
  var valueLabelComp = "";
  var valuesElem = "";

  var limitWord = "-;-";
  var limProComp = "-,-";

  jQuery.noConflict();

  for (var i = 0; i < elementsArrayLabel.length; i++) {
    var element = elementsArrayLabel[i];
    var idElem = element.id;
    var idLabel = element.id.substring(4);
    var currentDivComp = jQuery('#' + idElem);

    var componentType = currentDivComp.attr('componentType');
    switch (componentType) {
    case "LABEL":
      stylesElem = stylesElem
          + idLabel
          + limProComp
          + removeCssSpaces(replaceTop(idElem, element.style.cssText,
              skeepReplaceTop)) + limitWord;
      containerStyles = containerStyles
          + idLabel
          + limProComp
          + removeCssSpaces(replaceTop(idElem, element.style.cssText,
              skeepReplaceTop)) + limitWord;
      var textValue = element.textContent;
      if (textValue === undefined) {
        textValue = element.innerText;
      }
      if (textValue === undefined) {
        textValue = element.getElementsByTagName("td")[0].innerHTML;
      }
      valuesElem = valuesElem + idLabel + limProComp
          + escapeSeparators(textValue) + limitWord;
      break;

    case "LINE":
    case "FRAME":
    case "TABLE":
      stylesElem = stylesElem + idLabel + limProComp
          + removeCssSpaces(document.getElementById(idLabel).style.cssText)
          + limitWord;
      containerStyles = containerStyles
          + idLabel
          + limProComp
          + removeCssSpaces(replaceTop(idElem, element.style.cssText,
              skeepReplaceTop)) + limitWord;
      break;

    case "LINKPOP":
    case "LINK":
      var elementLink = document.getElementById(idLabel);
      stylesElem = stylesElem + idLabel + limProComp
          + removeCssSpaces(elementLink.style.cssText) + limitWord;
      containerStyles = containerStyles
          + idLabel
          + limProComp
          + removeCssSpaces(replaceTop(idElem, element.style.cssText,
              skeepReplaceTop)) + limitWord;
      valuesElem = valuesElem + idLabel + limProComp + elementLink.href
          + limitWord;
      var textLabelComp = elementLink.innerText;
      if (textLabelComp === undefined) {
        textLabelComp = elementLink.text;
      }
      if (textLabelComp === undefined) {
        textLabelComp = elementLink.textContent;
      }
      if (textLabelComp) {
        valueLabelComp = valueLabelComp + idLabel + limProComp
            + escapeSeparators(textLabelComp) + limitWord;
      }
      break;

    case "IMAGE":
      var imageElem = document.getElementById(idLabel);
      stylesElem = stylesElem + idLabel + limProComp
          + removeCssSpaces(imageElem.style.cssText) + limitWord;
      containerStyles = containerStyles
          + idLabel
          + limProComp
          + removeCssSpaces(replaceTop(idElem, element.style.cssText,
              skeepReplaceTop)) + limitWord;
      valuesElem = valuesElem + idLabel + limProComp + imageElem.src
          + limitWord;
      break;

    default:
      break;

    }
  }

  var elementsArray = getAllElementsInsideMainForm();

  for (var j = 0; j < elementsArray.length; j++) {
    var elemInput = elementsArray[j];
    var idElemInput = elemInput.id;

    // this stored the values for all components of type text, radio, checkbox,
    // select-multiple and textarea
    if (idElemInput && idElemInput.substring(0, 3) === "FB_") {
      var valueCompo = null;
      switch (elemInput.type) {
      case "text":
      case "textarea":
        // only add values for disabled textfield/textarea; enabled inputs are
        // transmitted via regular FORM parameters
        if (elemInput.disabled) {
          valueCompo = elemInput.value;
        }
        break;

      case "radio":
      case "checkbox":
        valueCompo = elemInput.value + "," + elemInput.checked;
        break;

      case "select-one":
        if (elemInput.selectedIndex != -1) {
          valueCompo = elemInput[elemInput.selectedIndex].value;
        }
        break;

      case "select-multiple":
        valueCompo = "";
        for (var k = 0; k < elemInput.options.length; k++) {
          if (elemInput.options[k].selected) {
            valueCompo = valueCompo + elemInput.options[k].value + ",";
          }
        }
        if (valueCompo == "") {
          valueCompo = null;
        }
        break;

      default:
        break;
      }
      if (valueCompo !== null && valueCompo !== "") {
        valuesElem = valuesElem + idElemInput + limProComp + valueCompo
            + limitWord;
      }

      // label values only for checkboxes and radio buttons
      if (elemInput.type === "checkbox" || elemInput.type === "radio") {
        var arrayLabels = document.getElementsByTagName("label");
        if (arrayLabels && arrayLabels.length > 0) {
          var labelComp = "";
          for (var m = 0; m < arrayLabels.length; m++) {
            if (arrayLabels[m].getAttribute("for") === idElemInput
                || arrayLabels[m].htmlFor === idElemInput) {
              labelComp = arrayLabels[m].innerHTML;
              break;
            }
          }
          if (labelComp !== "") {
            valueLabelComp = valueLabelComp + idElemInput + limProComp
                + escapeSeparators(labelComp) + limitWord;
          }
        }
      }

      if ("true" === elemInput.getAttribute("mandatory")) {
        mandatoryElem = mandatoryElem + idElemInput + limitWord;
      }
      if (elemInput.disabled || "true" === elemInput.disabled
          || "disabled" === elemInput.disabled) {
        disabledElem = disabledElem + idElemInput + limitWord;
      }
      if ("readonly" === elemInput.getAttribute("readonly")) {
        readOnlyElem = readOnlyElem + idElemInput + limitWord;
      }
      stylesElem = stylesElem + idElemInput + limProComp
          + removeCssSpaces(elemInput.style.cssText) + limitWord;

      var containerElem = document.getElementById("DIV_" + elemInput.id);
      if (containerElem) {
        containerStyles = containerStyles
            + idElemInput
            + limProComp
            + removeCssSpaces(replaceTop(containerElem.id,
                containerElem.style.cssText, skeepReplaceTop)) + limitWord;
      }
    }
  }

  document.getElementById("valueComponents").value = valuesElem;
  document.getElementById("labelValuesComp").value = valueLabelComp;
  document.getElementById("styleValues").value = stylesElem;
  document.getElementById("containerStyleValues").value = containerStyles;
  document.getElementById("mandatoryValues").value = mandatoryElem;
  document.getElementById("disabledValues").value = disabledElem;
  document.getElementById("readOnlyValues").value = readOnlyElem;

  collectReasonCodeComponents();
  collectRemoveAssignmentRadios();
}

function collectReasonCodeComponents() {
  var reasonString = ",";
  var comboboxes = document.getElementsByTagName("select");
  var sizeCombos = comboboxes.length;
  if (sizeCombos > 0) {
    for (var i = 0; i < sizeCombos; i++) {
      var reasonCode = comboboxes[i].getAttribute("reason-code");
      if (reasonCode == "true") {
        reasonString = reasonString + comboboxes[i].id + ",";
      }
    }
  }
  document.getElementById("reasonComponents").value = reasonString;
}

function collectRemoveAssignmentRadios() {
  var removeRIAString = ",";
  var inputs = document.getElementsByTagName("input");
  var sizeInputs = inputs.length;
  if (sizeInputs > 0) {
    for (var i = 0; i < sizeInputs; i++) {
      var elemInput = inputs[i];
      if (elemInput.type == "radio"
          && elemInput.getAttribute("removeassignment") == "yes") {
        removeRIAString = removeRIAString + elemInput.getAttribute("name")
            + ":" + elemInput.getAttribute("value") + ",";
      }
    }
  }
  document.getElementById("removeAssgnmntComponents").value = removeRIAString;
}

function fillMinimumValSequenceToContainer(sequenceMap, minValSeq,
    sequenceValToContainer) {
  var sequenceName = "";
  var containerName = "";
  var minValueSequence = "";
  var seqName = "_seqName:";
  var contName = "_contName:";
  var limitSeqItem = "-;-";
  var allSequenceMinVal = "";

  for (var i = 0; i < sequenceMap.length; i++) {
    for (var j = 0; j < sequenceMap[i].Value.length; j++) {
      sequenceName = sequenceMap[i].Name;
      containerName = sequenceMap[i].Value[j].Name;
      minValueSequence = sequenceMap[i].Value[j].Value;

      allSequenceMinVal = allSequenceMinVal + seqName + sequenceName + contName
          + containerName + minValSeq + minValueSequence + limitSeqItem;
    }
  }

  document.getElementById(sequenceValToContainer).value = allSequenceMinVal;
}

// Remove the "disabled" property for "radio", "checkBox" and comboBox
// components to simulate a "readOnly" property
// obviously if the component
// is marked as "readOnly", it is necessary because the "readOnly" property is
// not supported in html for "comboBox",
// "radio" and "checkBox".
function removeDisabledToReadOnlyComponents() {

  var elementsArray = getAllElementsInsideMainForm();
  for (var i = 0; i < elementsArray.length; i++) {
    var element = elementsArray[i];
    var readOnly = element.getAttribute("readonly");
    var disabled = element.disabled;
    ;
    var type = element.type;

    if ((type == "radio" || type == "checkbox" || type == "select-multiple" || type == "select-one")
        && readOnly == "readonly" && disabled) {
      element.disabled = false;
    }
  }
}

function getAllElementsInsideMainForm() {
  return jQuery(jQuery(".renderContainer")[0]).closest('form')[0].elements;
}

/**
 * Function to determine if DOM element identified by elementId would be removed
 * in Remove Assignment Process. This method return true if and only if the
 * element identified by elementId would not be assigned due to the Remove
 * Assignment Process.
 */
function wouldBeRemoved(elementId, $removeAssignmentCheckedRadios) {
  var varLength = $removeAssignmentCheckedRadios.length;
  if (varLength == 0) {
    return false;
  }

  var regex = /.*ERIK_(-?[0-9]*).*ENT_(-?[0-9]*).*RI_(-?[0-9]*).*/g;
  var regexResulElement = regex.exec(elementId);// 0
  var erikElement = regexResulElement[1];// capturing group 1
  var entityElement = regexResulElement[2];// capturing group 2
  var riElement = regexResulElement[3];// capturing group 3
  if (erikElement == null || entityElement == null || riElement == null) {
    return false;
  }

  for (var i = 0; i < varLength; i++) {
    var id = jQuery($removeAssignmentCheckedRadios[i]).attr('id');
    regex = /.*ERIK_(-?[0-9]*).*ENT_(-?[0-9]*).*RI_(-?[0-9]*).*/g;
    var regexResulId = regex.exec(id);
    var erikRadio = regexResulId[1];// capturing group 1
    var entityRadio = regexResulId[2];// capturing group 2
    var riRadio = regexResulId[3];// capturing group 3

    if (erikRadio == null || entityRadio == null || riRadio == null) {
      continue;
    }

    if (erikElement === erikRadio && riElement === riRadio
        && entityElement === entityRadio) {
      return true;
    }
  }

  return false;
}

function validateMandatories(arrayVisibleERI, arrayNotVisibleERI) {
  var isError = false;
  var arrayElements = getAllElementsInsideMainForm();
  var validRadioGroups = new Array();
  var validCheckGroups = [];
  var invalidCheckGroups = [];
  var invalidRadioGroups = new Array();
  var hiddenMandatoryInputs = new Array();
  var invalidMandatoryInputsCount = 0;

  // get the removeassignment elements checked
  var $removeAssignmentCheckedRadios = jQuery('input[type=radio][removeassignment=yes]:checked');

  for (var i = 0; i < arrayElements.length; i++) {
    var isValid = false;
    var element = arrayElements[i];
    var elementId = element.id;
    var mandatory = element.getAttribute("mandatory");
    var type = element.type;

    var $elementDiv = jQuery('#DIV_' + elementId);
    if (elementId.substring(0, 11) == ("tf_FB_TXTB_")) {
      $elementDiv = jQuery('#DIV_' + elementId.substring(3));
    }

    if (mandatory == "true") {
      // skip the mandatory validation
      if (!necessaryToValidate(elementId, arrayVisibleERI, arrayNotVisibleERI)
          || wouldBeRemoved(elementId, $removeAssignmentCheckedRadios)) {
        if ($elementDiv.children('a.mandatoryTooltip').length > 0) {
          removeMandatoryIcon($elementDiv[0]);
        }
        continue;
      }

      if (type == "radio") {
        // validation for element.type == radio
        var radioGroupName = element.name;
        if (!contains(validRadioGroups, radioGroupName)
            && !contains(invalidRadioGroups, radioGroupName)) {
          var $radios = jQuery('input[type=radio][name=' + radioGroupName
              + ']:checked');
          if ($radios.length > 0) {
            isValid = true;
            validRadioGroups[validRadioGroups.length] = radioGroupName;
          } else {
            invalidRadioGroups[invalidRadioGroups.length] = radioGroupName;
            createMandatoryIcon($elementDiv[0], element);
            isError = true;
          }
        } else if (contains(invalidRadioGroups, radioGroupName)) {
          createMandatoryIcon($elementDiv[0], element);
          isError = true;
        } else if (contains(validRadioGroups, radioGroupName)) {
          isValid = true;
        }

      } else if (type === "checkbox" && element.checked === false) {

        var checkGroupName = element.name;

        if (!contains(validCheckGroups, checkGroupName)
            && !contains(invalidCheckGroups, checkGroupName)) {

          var $allChecks = jQuery("input[group*='"
              + jQuery(element).attr('group') + "']");

          var checkeds = $allChecks
              .filter(function(index, check) {
                return jQuery(check).is(":checked");
              });

          if (checkeds.length === 0) {
            createMandatoryIcon($elementDiv[0], element);
            isError = true;
          } else {
            isValid = true;
          }

        } else if (contains(invalidCheckGroups, checkGroupName)) {
          createMandatoryIcon($elementDiv[0], element);
          isError = true;
        } else if (contains(validCheckGroups, checkGroupName)) {
          isValid = true;
        }

      } else if (type == "select-multiple" || type == "select-one") {
        // validation for element.type == select
        var options = element.options;
        for (var j = 0; j < options.length; j++) {
          if (options[j].selected && options[j].value != "-1") {
            isValid = true;
            break;
          }
        }
        if (!isValid) {
          createMandatoryIcon($elementDiv[0], element);
          isError = true;
        }
      } else {
        if (element.value.length == 0) {
          createMandatoryIcon($elementDiv[0], element);
          isError = true;
        } else {
          isValid = true;
        }
      }
      if (isValid) {
        if ($elementDiv.children('a.mandatoryTooltip').length > 0) {
          removeMandatoryIcon($elementDiv[0]);
        }
      } else {
        invalidMandatoryInputsCount++;
        if ($elementDiv.css('display') == "none") {
          if (type == "radio") {
            hiddenMandatoryInputs.push(radioButtonNamesMap[elementId]);
          } else {
            hiddenMandatoryInputs.push(componentsNamesMap[elementId]);
          }
        }
      }
    }
  }

  if (isError) {
    var msg = "";
    if (invalidMandatoryInputsCount > hiddenMandatoryInputs.length) {
      msg = "Some of the mandatory fields are not filled yet, they are marked with a red icon.";
    }
    if (hiddenMandatoryInputs.length > 0) {
      var joinMsg = hiddenMandatoryInputs.join(', ');
      msg = msg
          + "<br>"
          + "Hidden but mandatory fields are empty ("
          + joinMsg
          + "). Please contact the author of the form in order to fix this problem.";
    }

    jQuery('#errorMessage').html(msg);
    jQuery('#errorComponents').css('display', '');
    /* executeJqueryTooltip(); */
    window.scrollTo(10, 10);
    return false;
  }
  return true;
}

function executeJqueryTooltip() {
  jQuery(function() {
    jQuery(".mandatoryTooltip").tooltip({
      track : true,
      delay : 0,
      showURL : false,
      fixPNG : true,
      showBody : " - ",
      extraClass : "pretty fancy",
      top : 2,
      left : 3
    });
    jQuery('#pretty').tooltip({
      track : true,
      delay : 0,
      showURL : false,
      showBody : " - ",
      extraClass : "pretty",
      fixPNG : true,
      left : -120
    });
  });
}

function createMandatoryIcon(div, element) {
  var titleMessage = "";
  var message = "";

  var groupName = null;
  if (element.type == "radio") {
    var groupName = componentsNamesMap[element.getAttribute("id")];
  }

  if (element.type == "radio" && groupName != "null") {
    titleMessage = "Radio Group Mandatory ";
    if (groupName.length > 28) {
      groupName = groupName.substring(0, 28) + "\n" + groupName.substring(28);
    }
    message = " The radio group \n " + groupName
        + " is mandatory: one of the options must be selected.";
  } else {
    titleMessage = "Value Mandatory ";
    message = " A value for this field is required.";
  }

  if (jQuery(div).children('a.mandatoryTooltip').length > 0) {
    removeMandatoryIcon(div);
  }

  var idRadDiv = "DIV_FB_RADBTN_";

  var $elementLink = jQuery('<a>', {
    'href' : '#',
    'class' : 'mandatoryTooltip',
    'title' : (titleMessage + "-" + message)
  });
  $elementLink.click(function() {
    return false;
  });
  var $imgElement = jQuery('<img>', {
    'type' : 'image',
    'name' : 'image',
    'width' : '15px',
    'height' : '15px',
    'src' : mandatory_Icon
  });

  if (div.id.substring(0, 14) != (idRadDiv)) {
    jQuery(div).prepend($elementLink);
    $elementLink.attr('style',
        'position:absolute; z-index:1; top:0px; left:2px;');
    $elementLink.append($imgElement);
  } else {
    var widthDiv = div.style.width;
    widthDiv = (widthDiv.indexOf("px") != -1 || widthDiv.indexOf("PX") != -1) ? widthDiv
        .replace("px", '')
        : widthDiv;
    var widthNum = parseInt(widthDiv);
    widthNum = widthNum > 16 ? widthNum - 16 : widthNum;

    jQuery(div).append($elementLink);
    $elementLink.attr('style', ('position:absolute; z-index:1; top:0px; left:'
        + widthNum + 'px;'));
    $elementLink.append($imgElement);
  }
}

function removeMandatoryIcon(div) {
  var idRadDiv = "DIV_FB_RADBTN_";
  if (div.id.substring(0, 14) == (idRadDiv)) {
    div.lastChild.removeChild(div.lastChild.firstChild)
    div.removeChild(div.lastChild);
  } else {
    div.firstChild.removeChild(div.firstChild.firstChild)
    div.removeChild(div.firstChild);
  }
}

function necessaryToValidate(idComponent, arrayVisibleERI, arrayNotVisibleERI) {

  var eventRIKey = "";
  if (idComponent.indexOf("_ERIK_") > -1 && idComponent.indexOf("_ENT_") > -1) {
    eventRIKey = idComponent.split("_ERIK_")[1].split("_ENT_")[0];
  }

  if (eventRIKey !== ""
      && (arrayVisibleERI.length > 0 || arrayNotVisibleERI.length > 0)) {
    var containVisible = containsParsing(arrayVisibleERI, eventRIKey);
    var containNotVisible = containsParsing(arrayNotVisibleERI, eventRIKey);
    if ((!containVisible && !containNotVisible) || containVisible) {
      return true;
    }
    return false;

  } else {
    return true;
  }
}

function containsParsing(list, value) {
  for (var i = 0; i < list.length; i++) {
    if (parseInt('' + list[i]) === parseInt('' + value)) {
      return true;
    }
  }
  return false;
}

function validateDatesFormat(arrayVisibleERI, arrayNotVisibleERI) {
  var datePickerComponents = document.getElementsByClassName("datePickerClass");
  var isCorrectDate = true;
  var isCorrectDateTime = true;

  if (datePickerComponents != null && datePickerComponents.length > 0) {
    for (var j = 0; j < datePickerComponents.length; j++) {
      var idComponent = datePickerComponents[j].id;
      if (!necessaryToValidate(idComponent, arrayVisibleERI, arrayNotVisibleERI)) {
        continue;
      }
      var value = datePickerComponents[j].value;

      if (datePickerComponents[j].getAttribute("istime") === "true") {
        isCorrectDateTime = (isCorrectDateTime && validateDateTime(value));
      } else {
        isCorrectDate = (isCorrectDate && validateDate(value));
      }
    }
  }

  if (!isCorrectDate || !isCorrectDateTime) {
    var messageDate = "Some dates doesn't match the format: " + dateFormat;
    var messageDateTime = "Some date times doesn't match the format: "
        + dateTimeFormat;
    var message = "";
    if (!isCorrectDate) {
      message = messageDate;
    }
    if (!isCorrectDateTime) {
      if (message.length > 0) {
        message = message + "<br>";
      }
      message = message + messageDateTime;
    }

    document.getElementById("errorMessage").innerHTML = message;
    document.getElementById("errorComponents").style.display = '';
    window.scrollTo(10, 10);
    return false;
  }
  return true;
}

function validateDate(strValue) {
  var strFormat = javaDateFormat;

  if (strValue.length == 0) {
    return true;
  }

  if (strValue.length != 10) {
    return false;
  }

  var index = strFormat.indexOf("yyyy");
  var year = strValue.substr(index, 4);

  index = strFormat.indexOf("MM");
  var month = strValue.substr(index, 2);

  index = strFormat.indexOf("dd");
  var day = strValue.substr(index, 2);

  return validateNumber(year) && validateMonth(month) && validateDay(day);
}

function validateDateTime(strValue) {
  if (strValue.length === 0) {
    return true;
  }

  if (strValue.length === 16 || strValue.length === 19) { // Hardcoded Length ->
    // Assuming something
    // like ##/##/## ##:##
    // with any separator
    // and any
    // format.
    return (validateDate(strValue.substr(0, 10)) && validateTime(strValue
        .substr(11, 5)));
  }

  return false;
}

function validateTime(strValueTime) {
  var strFormat = dateTimeFormat.substr(11, 5);

  var index = strFormat.indexOf("HH");
  var hours = strValueTime.substr(index, 2);

  index = strFormat.indexOf("SS");
  var seconds = strValueTime.substr(index, 2);

  return validateHours(hours) && validateSeconds(seconds);
}

function validateNumber(number) {
  var isnum = /^\d+$/.test(number);
  return isnum;
}

function validateMonth(month) {
  return validateNumber(month) && month > 0 && month <= 12;
}

function validateDay(day) {
  return validateNumber(day) && day > 0 && day <= 31;
}

function validateHours(hours) {
  return validateNumber(hours) && hours >= 0 && hours <= 24;
}

function validateSeconds(time) {
  return validateNumber(time) && time >= 0 && time <= 59;
}

function contains(list, value) {
  for (var i = 0; i < list.length; i++) {
    if (list[i] === value) {
      return true;
    }
  }
  return false;
}

// This function is used for the unselect behavior on RadioButton
function unselectRadio(radioId, groupName) {
  for (var i = 0; i < currentRadioSelectedByGroupMap.length; i++) {
    if (currentRadioSelectedByGroupMap[i].groupName == groupName) {
      var currentRadioSelectedId = currentRadioSelectedByGroupMap[i].radioId;
      var radioSelected = document.getElementById(currentRadioSelectedId);
      currentRadioSelectedByGroupMap[i].radioId = radioId;

      if (radioSelected.onclick != null) {
        radioSelected.onclick();
      }
      return;
    }
  }

  var radioItemSelected = new Object();
  radioItemSelected.groupName = groupName;
  radioItemSelected.radioId = radioId;
  currentRadioSelectedByGroupMap.push(radioItemSelected);
}

function processBehavior(idSource, idTarget, typeBehavSource, affirmBehavior,
    textSource, isSelect, behavior, itemFromCombo) {

  var elementName = componentsNamesMap[idTarget];

  var groupTargets = jQuery("input[group*='" + elementName + "']");

  if (groupTargets.length !== 0) {
    groupTargets.each(function(index, target) {

      var idElementTarget = jQuery(target).attr('id')

      actionProcessBehavior(idSource, idElementTarget, typeBehavSource,
          affirmBehavior, textSource, isSelect, behavior, itemFromCombo);

    })

    return;
  }

  return actionProcessBehavior(idSource, idTarget, typeBehavSource,
      affirmBehavior, textSource, isSelect, behavior, itemFromCombo);
}

/**
 * Short version of processBehavior, where not a real ID is given but only the
 * index to the behaviorComponentNames array.
 */
function pB(sourceIdIndex, targetIdIndexes, typeBehavSource, affirmBehavior,
    textSource, isSelect, behavior, itemFromCombo) {
  var sourceId = behaviorComponentNames[sourceIdIndex];
  var targetIds = targetIdIndexes.split(",");
  for (var i = 0; i < targetIds.length; i++) {
    var targetId = behaviorComponentNames[targetIds[i]];
    processBehavior(sourceId, targetId, typeBehavSource, affirmBehavior,
        textSource, isSelect, behavior, itemFromCombo);
  }
}

function actionProcessBehavior(idSource, idTarget, typeBehavSource,
    affirmBehavior, textSource, isSelect, behavior, itemFromCombo) {

  var objSource = document.getElementById(idSource);

  var objTarget = document.getElementById(idTarget);

  var divTarget = document.getElementById("DIV_" + idTarget);
  var isLabel = false;
  var isAffirBehavior = true;
  var executeBehavior = false;

  if (objTarget == null) {
    objTarget = document.getElementById("DIV_" + idTarget);
    if (objTarget != null) {
      isLabel = true;
    }
  }

  var isSelected = false;
  if (isSelect != "null") {
    if (isSelect == "true") {
      isSelected = true;
    } else {
      isSelected = false;
    }
  }

  if (affirmBehavior == "false") {
    isAffirBehavior = false;
  }

  // if component is textBox or textArea the typeBehSource is 1, if component
  // is checkBox or radioButton the typeBehSource is 2
  switch (typeBehavSource) {
  case "1":
    if ((isAffirBehavior && textSource == objSource.value)
        || (!isAffirBehavior && textSource != objSource.value)) {
      executeBehavior = true;
    }
    break;

  case "2":
    if ((isAffirBehavior && isSelected == objSource.checked)
        || (!isAffirBehavior && isSelected != objSource.checked)) {
      executeBehavior = true;
    }
    break;

  case "3":
    if (objSource.value != null && itemFromCombo != null) {
      if ((isAffirBehavior && objSource.value == itemFromCombo)
          || (!isAffirBehavior && objSource.value != itemFromCombo)) {
        executeBehavior = true;
      }
    }
    break;

  default:
    break;
  }

  // It is not possible the target be null in a behavior.
  if (executeBehavior && objTarget != null) {
    var arrayBehavior = behavior.split("-;-");

    for (var i = 0; i < arrayBehavior.length; i++) {
      var indexStart = arrayBehavior[i].indexOf("[") + 1;
      var indexEnd = arrayBehavior[i].indexOf("]");
      var action = arrayBehavior[i].substring(indexStart, indexEnd);
      var indexStarAct = arrayBehavior[i].indexOf(":") + 1;
      var valueAction = arrayBehavior[i].substring(indexStarAct);

      if (action != "") {
        applyBehavior(action, valueAction, objTarget, idTarget, divTarget,
            isLabel, true);
      }
    }

    if (objTarget.id.substring(0, 7) != (HTML_LINK_ID)) {
      if (objTarget.onchange != null) {
        objTarget.onchange();
      }
      if (objTarget.onclick != null) {
        objTarget.onclick();
      }
    }
  }
}

function synchronizeFormData(strEntityKeys, cleanLocalStorageData) {
  if (strEntityKeys != null) {
    var entity, allKeys, sizeKeys, arrayAllKeys;
    var componentForm, componentValue, styleComponentMap;
    var thereIsEventRISectionToProcess = false;

    var entityKeys = strEntityKeys.split(",");
    for (var index = 0; index < entityKeys.length; index++) {
      entity = entityKeys[index];

      // first styles (behaviors)
      var oldStyles = localStorage.getItem("styleComponents_" + entity);
      if (oldStyles) {
        oldStyles = mintDecompress(oldStyles);

        // process styles only when exist
        styleComponentMap = createMapStyles(oldStyles);
        applyBehaviorsInSynchronizeMode(styleComponentMap);
      } else {
        oldStyles = "";
      }
      if (document.getElementById("behaviorAffectCompId")) {
        document.getElementById("behaviorAffectCompId").value = oldStyles;
      }
      // include also styles for the offline comp
      if (document.getElementById("behaviorAffectCompId2")) {
        document.getElementById("behaviorAffectCompId2").value = oldStyles;
      }

      // now component values
      allKeys = localStorage.getItem("filledComponentKeys_" + entity);
      if (allKeys) {
        arrayAllKeys = mintDecompress(allKeys).split(';');
        sizeKeys = arrayAllKeys.length;

        for (var indexKey = 0; indexKey < sizeKeys; indexKey++) {
          componentForm = document.getElementById(arrayAllKeys[indexKey]);
          if (componentForm != null) {
            componentValue = localStorage.getItem(arrayAllKeys[indexKey]);
            if (componentForm.id.indexOf('_CHKB_') >= 0
                || componentForm.id.indexOf('_RADBTN_') >= 0) {
              if (componentValue) {
                componentForm.checked = componentValue == "false" ? false
                    : true;
              } else {
                componentForm.checked = false;
              }
            } else if (componentForm.name !== undefined
                && componentForm.name.indexOf('_COMB_') >= 0) {
              if (componentValue) {
                for (var i = 0; i < componentForm.length; i++) {
                  if (componentValue.indexOf(componentForm.options[i].value) >= 0) {
                    componentForm.options[i].selected = true;
                  } else {
                    componentForm.options[i].selected = false;
                  }
                }
              } else {
                for (var j = 0; j < componentForm.length; j++) {
                  componentForm.options[j].selected = false;
                }
              }
            } else {
              if (componentValue) {
                componentForm.value = componentValue;
                if (componentForm.id.match("^selectable_record_items_")) {
                  thereIsEventRISectionToProcess = true;
                }
              } else {
                componentForm.value = null;
              }
            }
          }
        }
      }

    }
    if (thereIsEventRISectionToProcess) {
      initProcessToHideAndShowRII();
    }
  }

  if (cleanLocalStorageData)
    clearLocalStorageData(strEntityKeys);
}

function createMapStyles(allStyles) {
  var styleComponentMap = new Array();
  if (allStyles != null && allStyles != "") {
    var sepBehavior = "-@-";
    var sepField = "-;-";
    var sepCssProp = ";-#-;";

    var styleByComponent = allStyles.split(sepBehavior);
    for (var i = 0; i < styleByComponent.length; i++) {
      var arrayValues = styleByComponent[i].split(sepField);
      var idTarget = arrayValues[0];
      var idProp = arrayValues[1];
      var changeValue = arrayValues[2];
      var foundItem = false;

      for (var j = 0; j < styleComponentMap.length; j++) {
        if (styleComponentMap[j].idComponent == idTarget) {
          var oldStyle = styleComponentMap[j].style;
          styleComponentMap[j].style = oldStyle + sepCssProp + idProp
              + sepField + changeValue;
          foundItem = true;
          break;
        }
      }

      if (!foundItem) {
        var componentBehavior = new Object();
        componentBehavior.idComponent = idTarget;
        componentBehavior.style = idProp + sepField + changeValue;
        styleComponentMap.push(componentBehavior);
      }
    }
  }
  return styleComponentMap;
}

function applyBehaviorsInSynchronizeMode(styleComponentMap) {
  var componentForm;
  var behaviorItem;
  var idComponent;
  var limitProp = "-;-";
  var limitCssProp = ";-#-;";

  for (var indexKey = 0; indexKey < styleComponentMap.length; indexKey++) {
    behaviorItem = styleComponentMap[indexKey];

    if (behaviorItem.style != "") {
      idComponent = behaviorItem.idComponent;
      componentForm = document.getElementById(idComponent);

      var isLabel = false;
      var divComponentForm = null;
      if (componentForm == null) {
        componentForm = document.getElementById("DIV_" + idComponent);
        divComponentForm = componentForm;
      } else {
        if (componentForm.id.substring(0, 4) == "DIV_") {
          divComponentForm = componentForm;
        } else {
          divComponentForm = document.getElementById("DIV_" + componentForm.id);
        }
      }

      if (componentForm != null) {
        if (componentForm.id.substring(0, 13) == "DIV_FB_LABEL_") {
          isLabel = true;
        }

        var arrayBehavior = behaviorItem.style.split(limitCssProp);
        for (var i = 0; i < arrayBehavior.length; i++) {
          var details = arrayBehavior[i].split(limitProp);
          applyBehavior(details[0], details[1], componentForm, idComponent,
              divComponentForm, isLabel, false);
        }
      }
    }
  }
}

function applyBehavior(action, valueAction, objTarget, idTarget, divTarget,
    isLabel, storeBehavior) {
  var addBehavior = false;
  var oldObjTarget;

  // It is not possible the target be null in a behavior.
  if (objTarget === null) {
    return;
  }

  switch (action) {

  case "TEXT":
    if (isLabel) {
      objTarget.getElementsByTagName("td")[0].innerHTML = valueAction;
      addBehavior = true;
    } else if (objTarget.type == "checkbox" || objTarget.type == "radio") {
      var arrayLabels = document.getElementsByTagName("label");
      if (arrayLabels != null && arrayLabels.length > 0) {
        for (var j = 0; j < arrayLabels.length; j++) {
          if (arrayLabels[j].getAttribute("for") == idTarget) {
            arrayLabels[j].innerHTML = valueAction;
            addBehavior = true;
            break;
          }
        }
      }
    } else {
      if (idTarget.substring(0, 7) == HTML_LINK_ID) {
        objTarget.innerText = valueAction;
      } else {
        objTarget.value = valueAction;
      }
      addBehavior = true;
    }
    break;

  case "READ ONLY":

    var type = objTarget.type;
    var affectedObjs = [];
    var buttonObj;
    if (type == "text") {
      if (objTarget.getAttribute("fortree")) {
        buttonObj = document.getElementById("CL_" + idTarget);
        if (buttonObj)
          affectedObjs.push(buttonObj);
        buttonObj = document.getElementById("OP_" + idTarget);
        if (buttonObj)
          affectedObjs.push(buttonObj);
      } else if (objTarget.getAttribute("class") == "periodPicker"
          || objTarget.getAttribute("class") == "datePickerClass") {
        buttonObj = document.getElementById("F_" + idTarget);
        if (buttonObj)
          affectedObjs.push(buttonObj);
      }
    }
    if (valueAction == "true") {
      objTarget.setAttribute("readonly", "readonly");
      if ((type == "radio" || type == "checkbox" || type == "select-multiple" || type == "select-one")) {
        objTarget.disabled = true;
      }
      if (affectedObjs.length > 0) {
        for (var i = 0; i < affectedObjs.length; i++) {
          affectedObjs[i].disabled = true;
        }
      }
    } else if (valueAction == "false") {
      objTarget.removeAttribute("readonly");
      if ((type == "radio" || type == "checkbox" || type == "select-multiple" || type == "select-one")) {
        objTarget.disabled = false;
      }
      if (affectedObjs.length > 0) {
        for (var i = 0; i < affectedObjs.length; i++) {
          affectedObjs[i].disabled = false;
        }
      }
    }

    break;

  case "CLEAR":
    if (objTarget.type == "text" || objTarget.type == "textarea") {
      objTarget.value = "";
      if (objTarget.type == "text" && objTarget.getAttribute("fortree")) {
        var treeText = document.getElementById("tf_" + idTarget);
        if (treeText) {
          treeText.value = "";
        }
      }
    } else if (objTarget.type == "select-one") {
      objTarget.value = "-1";
    } else if (objTarget.type == "select-multiple") {
      var opts = objTarget.options;
      objTarget.value = "";
      for (var i = 0, opt; opt = opts[i]; i++) {
        opt.selected = false;
      }
    } else if (objTarget.type == "radio") {
      objTarget.checked = false;
    }

    break;

  case "WIDTH":
    if (objTarget.type == "checkbox" || objTarget.type == "radio"
        || idTarget.substring(0, 7) == HTML_LINK_ID) {
      divTarget.style.width = valueAction + "px";
    } else {
      var lValueAction = valueAction;
      if (objTarget.type == "text") {
        var reduce;
        if (objTarget.getAttribute("fortree")) {
          var nameText = document.getElementById("tf_" + idTarget);
          if (nameText) {
            oldObjTarget = objTarget;
            objTarget = nameText;
          }
          reduce = 34;
        }
        if (objTarget.getAttribute("istime")) {
          reduce = 18;
        }
        if (reduce) {
          var intVal = parseInt(lValueAction);
          if (!isNaN(intVal)) {
            lValueAction = String(intVal - reduce);
          }
        }
      }
      objTarget.style.width = lValueAction + "px";
    }
    addBehavior = true;
    break;

  case "HEIGHT":
    var isRadioCkeckbox = objTarget.type == "checkbox"
        || objTarget.type == "radio";
    if (isRadioCkeckbox || idTarget.substring(0, 7) == HTML_LINK_ID) {
      divTarget.style.height = valueAction + "px";
      if (isRadioCkeckbox) {
        divTarget.getElementsByTagName("table")[0].style.height = valueAction
            + "px";
      }
    } else {
      if (objTarget.type == "text" && objTarget.getAttribute("fortree")) {
        var nameText = document.getElementById("tf_" + idTarget);
        if (nameText) {
          oldObjTarget = objTarget;
          objTarget = nameText;
        }
      }
      objTarget.style.height = valueAction + "px";
    }
    // change also table height, PDF workaround
    if (isLabel) {
      objTarget.getElementsByTagName("table")[0].style.height = valueAction
          + "px";
    }
    addBehavior = true;
    break;

  case "X":
    var objDivContentTarget = document.getElementById("DIV_" + idTarget);
    objDivContentTarget.style.left = valueAction + "px";
    addBehavior = true;
    break;

  case "Y":
    var objDivContentTarget = document.getElementById("DIV_" + idTarget);
    objDivContentTarget.style.top = valueAction + "px";
    addBehavior = true;
    break;

  case "VISIBLE":
    if (!storeBehavior
        && (valueAction === "block" || valueAction === "visible")) {
      valueAction = "true";
    } else if (!storeBehavior
        && (valueAction === "none" || valueAction === "hidden")) {
      valueAction = "false";
    }

    if (isLabel) {
      if (valueAction === "true") {
        objTarget.style.visibility = "visible";
        if (storeBehavior) {
          addBehAffectComp(idTarget, "VISIBLE", "visible");
        }
      } else {
        objTarget.style.visibility = "hidden";
        if (storeBehavior) {
          addBehAffectComp(idTarget, "VISIBLE", "hidden");
        }
      }
    } else {
      var objDivContentTarget = document.getElementById("DIV_" + idTarget);
      if (objDivContentTarget) {
        var affectedObjs = [];
        var oldObjTarget;
        if (objTarget.type == "text" && objTarget.getAttribute("fortree")) {
          var nameText = document.getElementById("tf_" + idTarget);
          if (nameText) {
            oldObjTarget = objTarget;
            objTarget = nameText;
          }
          var buttonObj = document.getElementById("CL_" + idTarget);
          if (buttonObj)
            affectedObjs.push(buttonObj);
          buttonObj = document.getElementById("OP_" + idTarget);
          if (buttonObj)
            affectedObjs.push(buttonObj);
        }
        affectedObjs.push(objTarget);

        if (valueAction == "true") {
          objDivContentTarget.style.visibility = "visible";
          if (objTarget.type != "checkbox" && objTarget.type != "radio") {

            var vDisplay = "block";
            // special handling of pickers, because block makes the button to
            // get a new line
            if (objTarget.type == "text"
                && (objTarget.getAttribute("class") == "periodPicker"
                    || objTarget.getAttribute("class") == "datePickerClass" || objTarget
                    .getAttribute("fortree"))) {
              vDisplay = "inline-block";
            }
            for (var i = 0; i < affectedObjs.length; i++) {
              affectedObjs[i].style.visibility = "visible";
              affectedObjs[i].style.display = vDisplay;
            }
          }
          if (storeBehavior) {
            addBehAffectComp(idTarget, "VISIBLE", "visible");
          }
        } else if (valueAction == "false") {
          objDivContentTarget.style.visibility = "hidden";
          if (objTarget.type != "checkbox" && objTarget.type != "radio") {
            objTarget.style.visibility = 'hidden';
            objTarget.style.display = "none";
          }
          if (storeBehavior) {
            addBehAffectComp(idTarget, "VISIBLE", "hidden");
          }
        }
      }
    }
    break;

  case "MANDATORY":
    objTarget.setAttribute("mandatory", valueAction);
    addBehavior = true;
    if (valueAction == "false") {
      var objDivContentTarget = document.getElementById("DIV_" + idTarget);
      if (objDivContentTarget.firstChild.localName == "a"
          && objDivContentTarget.firstChild.getAttribute("class") == "mandatoryTooltip") {
        removeMandatoryIcon(objDivContentTarget);
      } else if (objDivContentTarget.lastChild.localName == "a"
          && objDivContentTarget.lastChild.getAttribute("class") == "mandatoryTooltip") {
        removeMandatoryIcon(objDivContentTarget);
      }
    }
    break;

  case "ENABLED":
    if (valueAction == "true" || valueAction == "false") {
      var affectedObjs = [];
      if (objTarget.type == "text") {
        var buttonObj;
        if (objTarget.getAttribute("fortree")) {
          objTarget = null;
          buttonObj = document.getElementById("CL_" + idTarget);
          if (buttonObj)
            affectedObjs.push(buttonObj);
          buttonObj = document.getElementById("OP_" + idTarget);
          if (buttonObj)
            affectedObjs.push(buttonObj);
        } else if (objTarget.getAttribute("class") == "periodPicker"
            || objTarget.getAttribute("class") == "datePickerClass") {
          buttonObj = document.getElementById("F_" + idTarget);
          if (buttonObj)
            affectedObjs.push(buttonObj);
        }
      }
      if (objTarget)
        affectedObjs.push(objTarget);

      var valDisabled = (valueAction == "false");
      for (var i = 0; i < affectedObjs.length; i++) {
        affectedObjs[i].disabled = valDisabled;
      }
      addBehavior = true;
    }
    break;

  case "SELECTED":
    if (valueAction == "true") {
      objTarget.checked = true;
      addBehavior = true;
    } else if (valueAction == "false") {
      objTarget.checked = false;
      addBehavior = true;
    }
    break;

  case "URL":
    if (objTarget.tagName == "IMG") { // if is a image
      objTarget.src = valueAction;
      addBehavior = true;
    } else { // if is a Link
      objTarget.href = valueAction;
      addBehavior = true;
    }
    break;

  default:
    break;
  }

  // restore obj
  if (oldObjTarget) {
    objTarget = oldObjTarget;
  }

  if (addBehavior && storeBehavior) {
    addBehAffectComp(idTarget, action, valueAction);
  }
}

function addBehAffectComp(idTarget, changeProp, valueAction) {
  _implAddBehAffectComp("behaviorAffectCompId2", idTarget, changeProp,
      valueAction);

  _implAddBehAffectComp("behaviorAffectCompId", idTarget, changeProp,
      valueAction);
}

function _implAddBehAffectComp(componentId, idTarget, changeProp, valueAction) {
  var oldHiddenValue = document.getElementById(componentId);
  if (oldHiddenValue) {
    var sepBehavior = "-@-";
    var sepField = "-;-";

    var behStart = idTarget + sepField + changeProp + sepField;
    var behAffectCompId = behStart + valueAction;

    var oldValue = document.getElementById(componentId).value;
    if (oldValue != null && oldValue != "") {
      var newValue = "";

      // removes old values of the same component+prop
      var nextLimit;
      var nextI = oldValue.indexOf(behStart);
      while (nextI >= 0) {
        if (nextI > 0) {
          newValue += oldValue.substr(0, nextI);
        }
        oldValue = oldValue.substr(nextI);
        nextLimit = oldValue.indexOf(sepBehavior);
        if (nextLimit >= 0) {
          oldValue = oldValue.substr(nextLimit + 3);
        } else {
          oldValue = "";
        }
        nextI = oldValue.indexOf(behStart);
      }
      newValue += oldValue;

      // add the behavior separator when does not already end with it
      if (newValue.lastIndexOf(sepBehavior) != newValue.length
          - sepBehavior.length) {
        newValue = newValue + sepBehavior;
      }

      behAffectCompId = newValue + behAffectCompId;
    }

    document.getElementById(componentId).value = behAffectCompId;
  }
}

function openPopup(idPopupContainer, idFormContainer, idPopupBackground,
    idCompomLink) {
  var compLink = document.getElementById(idCompomLink);
  var comesFromALink = false;
  var comesFromATreeComp = false;
  if (idCompomLink.indexOf(HTML_LINK_POPUP_ID) >= 0) {
    comesFromALink = true;
  } else if (idCompomLink.indexOf("tf_FB_TXTB_") >= 0) {
    comesFromATreeComp = true;
  }
  if (comesFromALink
      || compLink.type === "text"
      || ((compLink.type === "radio" || compLink.type === "checkbox") && compLink.checked)) {
    var bgdiv = document.getElementById(idPopupBackground);
    bgdiv.style.display = "block";

    var formdiv = document.getElementById(idPopupContainer);
    formdiv.style.display = "block";

    formdiv.handlerobj = new moveablePopup(formdiv);

    var containerdiv = document.getElementById(idFormContainer);
    if (containerdiv && containerdiv.SavedInnerHTML) {
      containerdiv.innerHTML = containerdiv.SavedInnerHTML;
    }

    jQuery.noConflict();
    // The popup will be open below from component link
    // The popup always opens horizontally in the center of the form.
    // If the popup is wider than the form, it is placed on X = 0
    var divCompLink = document.getElementById("DIV_" + idCompomLink);
    if (comesFromATreeComp) {
      divCompLink = document.getElementById("DIV_" + idCompomLink.substring(3));
    }
    if (divCompLink) {
      var componentLinkTop = jQuery(divCompLink).offset().top;
      var $formdiv = jQuery(formdiv);
      $formdiv.offset({
        top : componentLinkTop + 20,
        left : 0
      });

      var popupWidth = $formdiv.width();
      var widthViewPort = Math.max(document.documentElement.clientWidth,
          window.innerWidth || 0);
      var elmnt = jQuery(".render-form-container");
      var scrollLeft = elmnt.length > 0 ? elmnt.scrollLeft() : jQuery(window)
          .scrollLeft();
      if (popupWidth < widthViewPort) {
        $formdiv.css({
          "left" : (Math.floor((widthViewPort - popupWidth) / 2) + scrollLeft)
              + "px"
        });
      } else {
        $formdiv.css({
          "left" : "0px"
        });
      }
    }
  }
}

function cleanComponentTree(idCompKey, idCompShow) {
  var compKey = document.getElementById(idCompKey);
  var compShow = document.getElementById(idCompShow);

  if (compKey && compShow) {
    var divNodoSelected = "div_name_" + compKey.value + "_" + idCompKey;
    var divSelLater = document.getElementById(divNodoSelected);

    if (divSelLater) {
      divSelLater.style.background = '';
      divSelLater.style.color = 'black';
    }
    compKey.value = "";
    if (compKey.onchange)
      compKey.onchange();

    compShow.value = "";
    if (compShow.onchange)
      compShow.onchange();
  }
}

function hidePopup(idPopupContainer, idPopupBackground) {
  var formdiv = document.getElementById(idPopupContainer);
  formdiv.style.display = "none";

  var bgdiv = document.getElementById(idPopupBackground);
  bgdiv.style.display = "none";
}

function moveablePopup(popupContainer) {
  var _div_obj = popupContainer;

  this.isIE = false;
  this.isNS = false;

  this.Init = function() {
    if (navigator.userAgent.indexOf("MSIE") >= 0
        || navigator.userAgent.indexOf("Opera") >= 0) {
      this.isIE = true;
    } else {
      this.isNS = true;
    }
  };

  _div_obj.onmousedown = function(event) {
    var x = 0;
    var y = 0;
    _this = this.handlerobj;

    if (_this.isIE) {
      x = window.event.clientX + document.documentElement.scrollLeft
          + document.body.scrollLeft;
      y = window.event.clientY + document.documentElement.scrollTop
          + document.body.scrollTop;
    } else {
      x = event.clientX + window.scrollX;
      y = event.clientY + window.scrollY;
    }

    var posicion = getAbsoluteElementPosition(this);
    var top = parseInt(this.style.top, 10);
    top = posicion.top;
    var client_y = y - top;
    if (!(client_y > 0 && client_y < 30)) {
      return;
    }
    _this.cursorStartX = x;
    _this.cursorStartY = y;
    _this.divStartX = parseInt(this.style.left, 10);
    _this.divStartY = parseInt(this.style.top, 10);

    if (this.handlerobj.isIE) {
      document.attachEvent("onmousemove", _this.onmousemove);
      document.attachEvent("onmouseup", _this.onmouseup);
      window.event.cancelBubble = true;
      window.event.returnValue = false;
    } else {
      document.addEventListener("mousemove", _this.onmousemove, true);
      document.addEventListener("mouseup", _this.onmouseup, true);
      event.preventDefault();
    }

    _FG_tracker = _this;
    _this._div_obj = this;
  };

  this.onmousemove = function(event) {
    _this = _FG_tracker;
    var x = 0;
    var y = 0;
    if (_this.isIE) {
      x = window.event.clientX + document.documentElement.scrollLeft
          + document.body.scrollLeft;
      y = window.event.clientY + document.documentElement.scrollTop
          + document.body.scrollTop;
    } else {
      x = event.clientX + window.scrollX;
      y = event.clientY + window.scrollY;
    }

    _this._div_obj.style.left = (_this.divStartX + x - _this.cursorStartX)
        + "px";
    _this._div_obj.style.top = (_this.divStartY + y - _this.cursorStartY)
        + "px";

    if (_this.isIE) {
      window.event.cancelBubble = true;
      window.event.returnValue = false;
    } else {
      event.preventDefault();
    }
  };

  this.onmouseup = function() {
    _this = _FG_tracker;
    if (_this.isIE) {
      document.detachEvent("onmousemove", _this.onmousemove);
      document.detachEvent("onmouseup", _this.onmouseup);
    } else {
      document.removeEventListener("mousemove", _this.onmousemove, true);
      document.removeEventListener("mouseup", _this.onmouseup, true);
    }
    _FG_tracker = null;
  };

  this.Init();
}

function getAbsoluteElementPosition(element) {
  if (typeof element == "string")
    element = document.getElementById(element);

  if (!element)
    return {
      top : 0,
      left : 0
    };

  var y = 0;
  var x = 0;
  while (element.offsetParent) {
    x += element.offsetLeft;
    y += element.offsetTop;
    element = element.offsetParent;
  }
  return {
    top : y,
    left : x
  };
}

window.size = function() {
  var w = 0;
  var h = 0;

  // IE
  if (!window.innerWidth) {
    // strict mode
    if (!(document.documentElement.clientWidth == 0)) {
      w = document.documentElement.clientWidth;
      h = document.documentElement.clientHeight;
    }
    // quirks mode
    else {
      w = document.body.clientWidth;
      h = document.body.clientHeight;
    }
  }
  // w3c
  else {
    w = window.innerWidth;
    h = window.innerHeight;
  }
  return {
    width : w,
    height : h
  };
};

window.center = function() {
  var hWnd = (arguments[0] != null) ? arguments[0] : {
    width : 0,
    height : 0
  };

  var _x = 0;
  var _y = 0;
  var offsetX = 0;
  var offsetY = 0;

  // IE
  if (!window.pageYOffset) {
    // strict mode
    if (!(document.documentElement.scrollTop == 0)) {
      offsetY = document.documentElement.scrollTop;
      offsetX = document.documentElement.scrollLeft;
    }
    // quirks mode
    else {
      offsetY = document.body.scrollTop;
      offsetX = document.body.scrollLeft;
    }
  }
  // w3c
  else {
    offsetX = window.pageXOffset;
    offsetY = window.pageYOffset;
  }

  _x = ((this.size().width - hWnd.width) / 2) + offsetX;
  _y = ((this.size().height - hWnd.height) / 2) + offsetY;

  return {
    x : _x,
    y : _y
  };
};

function insertError() {
  if (componentsErrorMap != null) {
    var prop1 = "_ENT_";
    var prop2 = "_RI_";
    var prop3 = "_PK";
    var msgShow = "";

    for ( var property in componentsErrorMap) {
      var compIdError = property;
      var msgError = componentsErrorMap[property];
      var entityKey = getEntity(compIdError, prop1, prop2);
      var RecItemKey = getEntity(compIdError, prop2, prop3);
      var entityName = "";
      var recordItemName = "";
      var componentTraineeLine = "<b>" + componentsNamesMap[compIdError]
          + "</b> ";

      for ( var entity in entityMap) {
        if (entity == entityKey) {
          entityName = entityMap[entity];
          break;
        }
      }

      for ( var ri in recordItemsMap) {
        if (ri == RecItemKey) {
          recordItemName = recordItemsMap[ri];
          break;
        }
      }

      if (entityName) {
        componentTraineeLine = "<i>" + componentTraineeLine
            + "</i> for entity <b>" + entityName + "</b>";
      }
      if (recordItemName) {
        componentTraineeLine = componentTraineeLine + " in record item <b>"
            + recordItemName + "</b>";
      }
      if (msgError) {
        componentTraineeLine = componentTraineeLine + " Error : <b>" + msgError
            + "</b>";
      }

      msgShow = msgShow + "<li>" + componentTraineeLine;
    }

    msgShow = msgShow + ".<br>";

    document.getElementById("errorMessage").innerHTML = msgShow;
    document.getElementById("errorComponents").style.display = '';
    window.scrollTo(10, 10);
  }
}

function insertErrordeIdentify() {
  thereIsError = true;
  var msg = "<b>The form cannot be loaded due to the following reason(s):</b>";
  msg = msg
      + "<ul><li>This form requires a de-identification set up that has not been found. Please change form design in order to remove de-identification requirement, or ask a system administration to set up a Location and Organization to be used as De-identified.</li></ul>";
  document.getElementById("errorMessage").innerHTML = msg;
  document.getElementById("errorComponents").style.display = '';
  window.scrollTo(10, 10);
}

// show error message when form is invalid
function insertErrorInvalidForm(isEntity, invalidSourceComponentForPopup) {
  thereIsError = true;
  var msg = "<b>The form could not be opened due to the following reason(s):</b>";
  if (invalidRefToMandNoDefValProp || invalidRefToProp) {
    if (invalidRefToProp) {
      msg = msg
          + "<ul><li>The form references the following properties not activated for the use case <i>"
          + invalidRefToProp[0] + "</i>: ";
      msg = msg + getGeneralPropertyErrorMessage(invalidRefToProp);
    }
    if (invalidRefToMandNoDefValProp) {
      msg = msg
          + "<ul><li>There are properties activated as mandatory and without default value for the <i>"
          + invalidRefToMandNoDefValProp[0] + "</i> "
          + "use case, then they must be filled by the form. However "
          + (invalidRefToMandNoDefValProp.length -1)
          + " of them are missing: ";
      msg = msg + getGeneralPropertyErrorMessage(invalidRefToMandNoDefValProp);
    }
  }
  else {
    msg = msg + "<ul><li>";
    if (isEntity) {
      msg = msg + "The entity form has no use case defined";
    } else if (invalidSourceComponentForPopup) {
      msg = msg
          + "The Link to popup component or component used as a link is not configured correctly";
    } else {
      msg = msg + "The form for this grading is incomplete";
    }
    msg = msg
        + " or some properties might be missing. Please complete the form using the Form Builder editor in WebAssistant Client.</li></ul>";
  }
  document.getElementById("errorMessage").innerHTML = msg;
  document.getElementById("errorComponents").style.display = '';
  window.scrollTo(10, 10);
}
 
/**
 * Function to generate the general message error for invalid properties.
 * @param invalidProp
 * @returns {String}
 */
function getGeneralPropertyErrorMessage(invalidProp){
  
  var msg = "";
  
  for (var i = 1; i < invalidProp.length; i++) {
    msg = msg + "<b>" + invalidProp[i] + "</b>";
    if (i < invalidProp.length - 1) {
      msg = msg + ", ";
    }
  }
  msg = msg
      + ".<br>Please use WebAssistant Client to adjust the form or check the activations for the ";
  if (invalidRefsSignOff) {
    msg = msg
        + "record items used in the form (typically referenced in PopUp components)";
  } else {
    msg = msg + "properties";
  }
  msg = msg + ".</li></ul>";
  
  return msg;
}

function insertErrorinConfirm(errorMsg) {
  var auxMsg = errorMsg.replace(/-;-/g, "<br>");
  var arrayMsg = auxMsg.split("-;@;-");
  var msg = "<b>" + arrayMsg[0] + "</b>";
  if (arrayMsg.length > 1) {
    msg = msg + "<ul><li>" + arrayMsg[1] + "</li></ul>";
  }

  document.getElementById("errorMessage").innerHTML = msg;
  document.getElementById("errorComponents").style.display = '';
  window.scrollTo(10, 10);
}

function getEntity(compIdError, prop1, prop2) {
  var ent = compIdError.split(prop1);
  return ent[ent.length - 1].split(prop2)[0];
}

function hideStandAloneButtons() {
  var buttonsDiv = document.getElementsByClassName("standAloneButtons");
  if (buttonsDiv) {
    for (var i = 0; i < buttonsDiv.length; i++) {
      buttonsDiv[i].style.display = "none";
    }
    document.getElementById('ProgressMsg').style.display = '';
  }
}

function setRotateStandalone(rotateStandalone) {
  this.rotateStandAlone = rotateStandalone;
}

function setRemoveSelectRIButton(removeSelectRIButton) {
  this.removeSelectRIButton = removeSelectRIButton;
}

function setSetEntityKeys(entityKeys) {
  this.setEntityKeys = entityKeys;
}

function moveStandAlone() {
  if (rotateStandAlone == true) {
    var displayErrors = document.getElementById("errorComponents").style.display;
    document.getElementById("errorComponents").style.display = 'none';

    clearTimeout(rotateTimeout);

    switch (window.orientation) {
    case 90:
    case -90:
      rotateTimeout = window.setTimeout(toHorizontal(), 400);
      break;
    case 0:
    case 180:
      rotateTimeout = window.setTimeout(toVertical(), 400);
      break;
    }

    if (displayErrors != 'none') {
      window
          .setTimeout(
              function() {
                document.getElementById("errorComponents").style.display = displayErrors;
              }, 600);
    }
  }
}

function getReduceValueToRotate(iteratorHeight, resourceArraySize) {

  var resourceDivSize = iteratorHeight / resourceArraySize;
  var rowsPerIterator = Math.ceil(resourceArraySize / 2);
  var sizeReduced = (resourceArraySize - rowsPerIterator) * resourceDivSize;
  return sizeReduced;

}

function getResourceKeys() {

  var resourceKeys = new Array();
  for (var i = 0; i < this.setEntityKeys.length; i++) {
    if (this.setEntityKeys[i].indexOf("_") > -1) {
      var element_Keys = this.setEntityKeys[i].split("_");
      if (element_Keys.length > 3) {
        resourceKeys.push(element_Keys[3]);
      }
    } else {
      resourceKeys.push(this.setEntityKeys[i]);
    }
  }
  return resourceKeys;
}

function toHorizontal() {

  if (lastOrientation == "landscape") {
    return;
  }

  // Get the Main div.
  var parentDiv = document.getElementById("divContentForm");

  // Get the divs
  var allDivs = parentDiv.getElementsByTagName("div");

  // First level childrens of the Parent div.
  var childrens = new Array();
  var count = 0;
  var biggestSize = 0;
  var iteratorsCount = 0;

  arrayHorizontalPositions = {};

  // Get the first level childrens.
  for (var i = 0; i < allDivs.length; i++) {
    if (allDivs[i].parentNode == parentDiv) {
      childrens[count++] = allDivs[i];
    }

    jQuery.noConflict();
    var currentDiv = jQuery(allDivs[i]);
    if (currentDiv.attr('componentType') === "SFITERATOR") {
      arrayIterators[iteratorsCount++] = allDivs[i];
      var leftPos = parseInt(allDivs[i].style.left);
      var divSize = ((allDivs[i].offsetWidth + leftPos) * 2);
      if (biggestSize < divSize) {
        biggestSize = divSize;
      }
    }
  }

  var resourceKeys = getResourceKeys();

  // Get the components below each other.
  for (var i = 0; i < childrens.length; i++) {
    for (var j = 0; j < arrayIterators.length; j++) {
      if (childrens[i] == arrayIterators[j]) {
        continue;
      }

      var yPos = parseInt(arrayIterators[j].style.top)
          + parseInt(arrayIterators[j].offsetHeight);

      if (parseInt(childrens[i].style.top) >= yPos) {
        if (arrayPositions[childrens[i].id] == null) {
          arrayPositions[childrens[i].id] = childrens[i].style.top;
        }

        var sizeReduced = getReduceValueToRotate(
            arrayIterators[j].offsetHeight, resourceKeys.length)

        if (arrayHorizontalPositions[childrens[i].id] == null) {
          arrayHorizontalPositions[childrens[i].id] = sizeReduced;
        } else {
          arrayHorizontalPositions[childrens[i].id] += sizeReduced;
        }
      }
    }
  }

  for ( var propname in arrayHorizontalPositions) {
    var element = document.getElementById(propname);
    element.style.top = (parseInt(element.style.top) - arrayHorizontalPositions[propname])
        + "px";
  }

  var newWidth = 0;
  var maxWidth = 0;
  var traineeDiv;
  var leftPosIterator;

  for (var i = 0; i < arrayIterators.length; i++) {
    if (arrayIterators[i].childNodes.length > 1) {
      leftPosIterator = parseInt(arrayIterators[i].style.left);
      traineeDiv = arrayIterators[i].firstChild;
      newWidth = traineeDiv.offsetWidth + leftPosIterator;
      if (newWidth > maxWidth) {
        maxWidth = newWidth;
      }
    }
  }

  reducedSize = 0;

  for (var i = 0; i < arrayIterators.length; i++) {

    if (arrayIterators[i].childNodes.length > 1) {
      var newRow = false;
      var topPosition = 0;
      var newTopPosition = 0;
      var topPositionNumber = 0;
      var leftStylePos;

      reducedSize += getReduceValueToRotate(arrayIterators[i].offsetHeight,
          resourceKeys.length);

      for (var j = 0; j < resourceKeys.length; j++) {
        var standTraineeDivName = "FB_ITERATOR_DIV_TRAINEE_ERIK_0_ENT_"
            + resourceKeys[j] + "_RI_0_PK_0";

        for (var k = 0; k < arrayIterators[i].childNodes.length; k++) {

          var traineeDiv = arrayIterators[i].childNodes[k];
          if (traineeDiv.id == standTraineeDivName) {
            if (j % 2 == 0) {
              topPosition = traineeDiv.style.top;
              // if the iteration is a pair, it has to be painted in a new row
              // or new height position.
              if (newRow) {
                traineeDiv.style.top = newTopPosition;
                topPosition = newTopPosition;
              }

            } else {
              traineeDiv.style.top = topPosition;
              leftStylePos = maxWidth + "px";
              traineeDiv.style.left = leftStylePos;
              // set the new position for the next iteration form
              topPositionNumber = topPositionNumber + traineeDiv.offsetHeight;
              newTopPosition = topPositionNumber + "px"
              newRow = true;
            }
            traineeDiv.style.position = 'absolute';
            break;
          }
        }
      }
    }
  }

  // workarounds for the iPad2+ rotate renderer glitch
  parentDiv.style.width = (biggestSize + 20) + "px";
  // recalculate new height for the parent div
  setTimeout(function() {
    parentDiv.style.height = (parentDiv.offsetHeight - reducedSize) + "px";
  }, 400);

  var widthWindow = jQuery(window).width() + "px";
  jQuery("#formRenderComponent").width(widthWindow);

  lastOrientation = "landscape";
}

function toVertical() {
  if (lastOrientation == "portrait") {
    return;
  }

  jQuery("#formRenderComponent").width("");
  // Get the Main div.
  var parentDiv = document.getElementById("divContentForm");

  // Set the position for components below to the iterator.
  for ( var propname in arrayPositions) {
    document.getElementById(propname).style.top = arrayPositions[propname];
  }

  var resourceKeys = getResourceKeys();

  // Remove the top position and change the left position for the iterators
  for (var i = 0; i < arrayIterators.length; i++) {
    var catchedValue = false;
    var leftPosition = 0;

    for (var j = 0; j < resourceKeys.length; j++) {
      var standTraineeDivName = "FB_ITERATOR_DIV_TRAINEE_ERIK_0_ENT_"
          + resourceKeys[j] + "_RI_0_PK_0";

      for (var k = 0; k < arrayIterators[i].childNodes.length; k++) {

        var traineeDiv = arrayIterators[i].childNodes[k];
        if (traineeDiv.id == standTraineeDivName) {
          if (!catchedValue) {
            leftPosition = traineeDiv.style.left;
            catchedValue = true;
          } else {
            traineeDiv.style.left = leftPosition;
          }
          traineeDiv.style.top = "";
          traineeDiv.style.position = 'relative';
          break;
        }
      }
    }
  }
  parentDiv.style.width = "";
  // recalculate new height for the parent div
  parentDiv.style.height = (parentDiv.offsetHeight + reducedSize) + "px";

  lastOrientation = "portrait";
}

function setTypeBehaviors(typeBehaviors) {
  this.typeBehaviors = typeBehaviors;
}

function validateTypeBehaviors() {
  var showMessage = false;
  var msg = "<b>Reason codes:</b><br /> ";
  if (typeBehaviors) {
    var sizeTypeBehaviors = typeBehaviors.length;
    for (var i = 0; i < sizeTypeBehaviors; i++) {
      var typeBehavior = typeBehaviors[i];
      if (typeBehavior && typeBehavior != "") {
        var conditions = typeBehavior.split(":");
        if (conditions) {
          var comboComponent = document.getElementById(conditions[0]);
          var radioComponent = document.getElementById(conditions[1]);
          var needReasonCode = conditions[2];
          var message = conditions[3];
          var radioChecked = radioComponent.checked;
          var comboHasSelection = comboSelectedOptions(comboComponent);

          if (radioChecked) {
            if (needReasonCode == "true") {
              if (comboHasSelection == false) {
                showMessage = true;
                msg = addInvalidReasonReason(comboComponent, msg, message);
              }
            } else {
              if (comboHasSelection == true) {
                showMessage = true;
                msg = addInvalidReasonReason(comboComponent, msg, message);
              }
            }
          }
        }
      }
    }
  }
  if (showMessage == true) {
    document.getElementById("errorMessage").innerHTML = msg;
    document.getElementById("errorComponents").style.display = '';
    window.scrollTo(10, 10);
    return false;
  }
  return true;
}

function addInvalidReasonReason(comboComponent, msg, message) {
  var keyValues = comboComponent.id.split("_");
  var entityKey = "";
  var recordItemKey = "";
  var entityName = "";
  var recordItemName = "";

  for (var j = 0; j < keyValues.length; j++) {
    if (keyValues[j] == "ENT") {
      entityKey = keyValues[j + 1];
    }

    if (keyValues[j] == "RI") {
      recordItemKey = keyValues[j + 1];
    }
  }
  for ( var property in entityMap) {
    if (property == entityKey) {
      entityName = entityMap[property];
      break;
    }
  }

  for ( var property in recordItemsMap) {
    if (property == recordItemKey) {
      recordItemName = recordItemsMap[property];
      break;
    }
  }

  msg = msg + "<li> For " + entityName + ", " + recordItemName
      + " is not correct";
  if (message) {
    msg = msg + ", " + message;
  }
  msg = msg + "<br />";

  return msg;
}

function verifyCheckboxesNoSelected() {
  fillMinimumValSequenceToContainer(sequenceRealValToContainerMap,
      "_minValSeq:", "sequenceRealValToContainer");
  fillValueSequenceToComponent();

  if (compChexkBoxYesNoMap) {
    var checkNoSelected = "";
    var limWord = "-;-";
    var limWordProComp = "-,-";

    for ( var property in compChexkBoxYesNoMap) {
      var compId = property;
      var realValue = compChexkBoxYesNoMap[property];
      var compCheckBox = document.getElementById(compId);

      if (compCheckBox != null && !compCheckBox.checked) {
        var oppositeValue = "";
        if (realValue == "YES") {
          oppositeValue = "NO";
        } else if (realValue == "NO") {
          oppositeValue = "YES";
        }

        checkNoSelected = checkNoSelected + compId + limWordProComp + "value:"
            + oppositeValue + limWord;
      }
    }
    document.getElementById("unselectedCheckComp").value = checkNoSelected;
  }
}

function fillValueSequenceToComponent() {
  if (sequenceComponentValueMap) {
    var checkNoSelected = "";
    var limWord = "-;-";
    var limWordProComp = "-,-";

    for ( var property in sequenceComponentValueMap) {
      var compId = property;
      var realSequenceValue = sequenceComponentValueMap[property];
      checkNoSelected = checkNoSelected + compId + limWordProComp + "value:"
          + realSequenceValue + limWord;
    }
    document.getElementById("sequenceValueComponent").value = checkNoSelected;
  }
}

function injectSymbol(text, symbol, position){
  if(position < 0 || position > 4)
    return text;
  return text.substr(0, position) + symbol + text.substr(position + 1);
}

// validate range for property type: Day, Month, Year
function validateNumbForTypeProp(compType, compId) {
  var validValMax = 0;
  var validValMin = 0;

  switch (compType) {
  case "DAY":
    validValMax = 31;
    break;
  case "MONTH":
    validValMax = 12;
    break;
  case "YEAR":
    validValMax = 8999;
    validValMin = 1899;
    break;
  }

  var comp = document.getElementById(compId);
  if (comp) {
    var compValue = parseInt(comp.value);
    if (compValue <= validValMin || compValue > validValMax) {
      comp.value = "";
    }
  }
}

function validateValuesForTime(evt) {
  evt.preventDefault();
  var keynum = window.event ? evt.keyCode : evt.which;
    
  var FIRST_DIGIT = 48;
  var FIRST_NUM = 96;
  
  var LEFT_ARROW = 37;
  var RIGHT_ARROW = 39;
  var BACKSPACE = 8;
  var DELETE = 46;
  
  var pressedNumber;
  var pos = evt.currentTarget.selectionStart;
  var val = evt.currentTarget.value;
  
  if(keynum >= FIRST_DIGIT && keynum < FIRST_DIGIT + 10)
    pressedNumber = keynum - FIRST_DIGIT;
  else if(keynum >= FIRST_NUM && keynum < FIRST_NUM + 10)
    pressedNumber = keynum - FIRST_NUM;
  else{
    if([LEFT_ARROW, BACKSPACE].indexOf(keynum) != -1)
      pos = Math.max(0, pos - (pos == 3 ? 2 : 1));
    if([RIGHT_ARROW].indexOf(keynum) != -1)
      pos = Math.min(4, pos + (pos == 1 ? 2 : 1));    
    if([DELETE, BACKSPACE].indexOf(keynum) != -1){
      evt.currentTarget.value = injectSymbol(val, '0', pos); 
    }
    evt.currentTarget.selectionStart = pos;
    evt.currentTarget.selectionEnd = pos;  
    return false;
  }
  
  if(pos == 0 && pressedNumber > 2)
    return false;
  if(pos == 0 && pressedNumber == 2 && val[1] > 3)
    val = injectSymbol(val, '3', 1);
    
  else if(pos == 1 && val[0] == 2 && pressedNumber > 3)
    return false;
  else if(pos == 2)
    pos = 3;
  
  if(pos == 3 && pressedNumber > 5)
    return false;
  else if(pos > 4)
    return false;

  val = injectSymbol(val, pressedNumber, pos);
  if(!isValidTime(val)){
    evt.currentTarget.value = evt.currentTarget.getAttribute("data-valid-value");
    return;
  }
  evt.currentTarget.value = val;
  evt.currentTarget.setAttribute("data-valid-value", val);
  
  pos++;
  if(pos == 2)
    pos = 3;
  evt.currentTarget.selectionStart = pos;
  evt.currentTarget.selectionEnd = pos;
  return false;
}

// validate range and format for property type: Time
function validateFormatTime(compId) {
  var comp = document.getElementById(compId);
  if (comp) {
    if(!isValidTime(comp.value))
      comp.value = comp.getAttribute("data-valid-value");
  }
}

function isValidTime(value){
  if(value == "")
    return true;
  var tokens = value.split(":");
  if (tokens.length == 2) {
    var valHH = parseInt(tokens[0]);
    var valMM = parseInt(tokens[1]);
    var lenghtHH = tokens[0].split("");
    var lenghtMM = tokens[1].split("");

    var maxValueHH = 23;
    var maxValueMM = 59;

    return (valHH >= 0 && valHH <= maxValueHH && valMM >= 0
        && valMM <= maxValueMM && lenghtHH.length == 2
        && lenghtMM.length == 2);
  }
}

function numbersOnly(evt) {
  var keynum;
  if (window.event) { // IE
    keynum = evt.keyCode;
  } else {
    keynum = evt.which;
  }
  return keynum <= 13 || keynum >= 48 && keynum <= 57;
}

function numbersFloatOnly(evt) {
  var keynum;
  if (window.event) { // IE
    keynum = evt.keyCode;
  } else {
    keynum = evt.which;
  }
  return keynum <= 13 || keynum >= 48 && keynum <= 57 || keynum == 46;
}

// validate only number for property type: Day, Month, Year
function numbersOnlyDates(evt) {
  var keynum;
  if (window.event) { // IE
    keynum = evt.keyCode;
  } else {
    keynum = evt.which;
  }
  return keynum <= 13 || keynum >= 48 && keynum <= 57;
}

function validatePeriodInvalid(period, idCompPeriod) {
  var startDate = period.selection.getFirstDate();
  var endDate = period.selection.getLastDate();
  if (startDate == undefined || endDate == undefined || startDate == endDate) {
    document.getElementById(idCompPeriod).value = "";
  }
}

function comboSelectedOptions(comboComponent) {
  if (comboComponent) {
    selectedOptions = new Array();
    if (comboComponent.options) {
      var sizeOptions = comboComponent.options.length;
      var indexSelected = 0;
      for (var k = 0; k < sizeOptions; k++) {
        if (comboComponent.options[k].selected) {
          selectedOptions[indexSelected] = comboComponent.options[k].value;
          if (selectedOptions[indexSelected] != "-1") {
            indexSelected++;
          }
        }
      }
      if (indexSelected > 0) {
        return true;
      }
    }
  }
  return false;
}

function createInfoBox(infoMessage) {
  return "<table class='generic' style='margin-bottom:15px; width:99%;'><tr>"
      + "<td style='padding:8px;' width='34'><img src='../../pics/information_icon_31x31.png'/></td>"
      + "<td style='padding:8px; vertical-align:middle;'><div><!-- INFO_MESSAGGE-->"
      + infoMessage + "<!-- INFO_MESSAGGE--></div></td>" + "</tr></table>";
}

function containsIntoArray(arrayObjects, objectToSearch) {
  if (arrayObjects) {
    var size = arrayObjects.length;
    for (var i = 0; i < size; i++) {
      if (arrayObjects[i] == objectToSearch) {
        return true;
      }
    }
  }
  return false;
}

function setDateFormat(dateFormat) {
  this.dateFormat = dateFormat;
}

function setJavaDateFormat(javaDateFormat) {
  this.javaDateFormat = javaDateFormat;
}

function setStateForm(stateForm) {
  this.stateForm = stateForm;
}

function confirmFormAction() {
  hideButtonsShowProgressMessages("Submit");
  verifyCheckboxesNoSelected();
  fillPropHtml(true);
}

function hideButtonBar() {
  var buttonsDiv = document.getElementsByClassName("standAloneButtons");
  if (buttonsDiv) {
    for (var i = 0; i < buttonsDiv.length; i++) {
      buttonsDiv[i].style.display = 'none';
    }
  }
}

function hideButtonsShowProgressMessages(actionType) {
  // hide buttons
  hideButtonBar();
  // show progress message
  if (actionType != null && document.getElementById('ProgressMsg' + actionType)) {
    document.getElementById('ProgressMsg' + actionType).style.display = '';
  } else {
    document.getElementById('ProgressMsg').style.display = '';
  }
}

function showButtonsHideProgressMessages() {
  var buttonsDiv = document.getElementsByClassName("standAloneButtons");
  if (buttonsDiv) {
    for (var i = 0; i < buttonsDiv.length; i++) {
      buttonsDiv[i].style.display = '';
    }
  }

  if (document.getElementById('ProgressMsg') != null) {
    document.getElementById('ProgressMsg').style.display = 'none';
  }
  if (document.getElementById('ProgressMsgSave') != null) {
    document.getElementById('ProgressMsgSave').style.display = 'none';
  }
  if (document.getElementById('ProgressMsgSubmit') != null) {
    document.getElementById('ProgressMsgSubmit').style.display = 'none';
  }
}

/**
 * Function called in the onchange event of the first component of the iteration
 * set, marked as autofilled. This propagates the value to the other components
 * of the set.
 */
function autofill(component) {
  if (!component || (component.type == "radio" && !component.checked)) {
    return; // skip when radio unselected (this is triggered programmatically)
  }

  // get the original form component name to iterate others
  var sourceCompId = component.id;

  // special case for hidden text fields (tree picker)
  var prefix = "";
  if (sourceCompId.indexOf("tf_") == 0) {
    prefix = "tf_";
    sourceCompId = sourceCompId.substring(3);
  }

  var sourceCompName = componentsNamesMap[sourceCompId];

  if (sourceCompName) {
    if (component.type == "radio") {

      // iterate all radios to group them by entity (iteration)!
      var radioGroups = {};
      var radioById, radioGroup;
      var sourceRadioGroup = component.name;
      for ( var compId in componentsNamesMap) {
        if (componentsNamesMap[compId] == sourceCompName) {
          radioById = document.getElementById(compId);
          if (radioById && radioById.name != sourceRadioGroup) {
            radioGroup = radioGroups[radioById.name];
            if (!(radioGroup)) {
              radioGroup = [];
              radioGroups[radioById.name] = radioGroup;
            }
            radioGroup.push(radioById);
          }
        }
      }

      // now iterate other similar groups to find current value of each
      // and determine if it must be replaced with the new value
      var newSourceCompValue = component.value;

      var autofilledValueAttr, curValue, radioComp;
      for ( var radioGroupName in radioGroups) {
        radioGroup = radioGroups[radioGroupName];
        autofilledValueAttr = radioGroup[0].getAttribute("autofilled-value");
        curValue = "";
        for (var index = 0; index < radioGroup.length; index++) {
          if (radioGroup[index].checked) {
            curValue = String(radioGroup[index].value);
          }
        }

        // criteria for replacement: no current value, or equals to last applied
        // in autofill
        if (curValue == ""
            || (autofilledValueAttr && autofilledValueAttr == curValue)) {
          for (var index = 0; index < radioGroup.length; index++) {
            radioComp = radioGroup[index];
            if (radioComp.value == newSourceCompValue) {
              radioComp.checked = true;
              if (radioComp.onclick) {
                radioComp.onclick();
              }
              if (radioComp.onchange) {
                radioComp.onchange();
              }
            }
          }
          radioGroup[0].setAttribute("autofilled-value", newSourceCompValue);
        }
      }

    } else {

      // iterate all component names to find similar ones!
      var similarCompsById = [];
      var compById;
      for ( var compId in componentsNamesMap) {
        if (sourceCompId != compId
            && componentsNamesMap[compId] == sourceCompName) {
          compById = document.getElementById(prefix + compId)
          if (compById) {
            similarCompsById.push(compById);
          }
        }
      }

      var targetComp, autofilledValueAttr, newSourceCompValue;

      switch (component.type) {
      case "text":
      case "textarea":
        newSourceCompValue = component.value;
        for (var index = 0; index < similarCompsById.length; index++) {
          targetComp = similarCompsById[index];
          // get last value autofilled
          autofilledValueAttr = targetComp.getAttribute("autofilled-value");
          if (!(targetComp.value)
              || (autofilledValueAttr && targetComp.value == autofilledValueAttr)) {
            // change only when no text or the current value is the same last
            // applied
            targetComp.value = newSourceCompValue;
            targetComp.setAttribute("autofilled-value", newSourceCompValue);
            if (targetComp.onchange) {
              targetComp.onchange();
            }
          }
        }
        break;

      case "checkbox":
        newSourceCompValue = component.checked;
        for (var index = 0; index < similarCompsById.length; index++) {
          targetComp = similarCompsById[index];
          // get last value autofilled
          autofilledValueAttr = targetComp.getAttribute("autofilled-value");
          if ((!(autofilledValueAttr) && !targetComp.checked)
              || (autofilledValueAttr && String(targetComp.checked) == autofilledValueAttr)) {
            targetComp.checked = newSourceCompValue;
            targetComp.setAttribute("autofilled-value", newSourceCompValue);
            if (targetComp.onclick) {
              targetComp.onclick();
            }
          }
        }
        break;

      case "select-one":
        newSourceCompValue = component.selectedIndex;
        for (var index = 0; index < similarCompsById.length; index++) {
          targetComp = similarCompsById[index];
          // get last value autofilled
          autofilledValueAttr = targetComp.getAttribute("autofilled-value");
          if (targetComp.selectedIndex == -1
              || targetComp.options[targetComp.selectedIndex].value == "-1"
              || (autofilledValueAttr && String(targetComp.selectedIndex) == autofilledValueAttr)) {
            targetComp.selectedIndex = newSourceCompValue;
            targetComp.setAttribute("autofilled-value", newSourceCompValue);
            if (targetComp.onchange) {
              targetComp.onchange();
            }
          }
        }
        break;

      case "select-multiple":
        newSourceCompValue = getSelectedIndexesStr(component);
        var curSelectedStr;
        for (var index = 0; index < similarCompsById.length; index++) {
          targetComp = similarCompsById[index];
          // get last value autofilled
          autofilledValueAttr = targetComp.getAttribute("autofilled-value");
          curSelectedStr = getSelectedIndexesStr(targetComp);
          if (curSelectedStr == ""
              || (autofilledValueAttr && curSelectedStr == autofilledValueAttr)) {
            selectSameIndexes(component, targetComp);
            targetComp.setAttribute("autofilled-value", newSourceCompValue);
            if (targetComp.onchange) {
              targetComp.onchange();
            }
          }
        }
        break;

      default:
        break;
      }
    }
  }
}

/**
 * Concatenates, comma separated, the selected option indexes of the given
 * component.
 */
function getSelectedIndexesStr(comp) {
  var str = "";
  if (comp.options) {
    for (var k = 0; k < comp.options.length; k++) {
      if (comp.options[k].selected) {
        str = str + k + ",";
      }
    }
  }
  return str;
}

/**
 * Receives two select components (comboboxes) and then selects in the target
 * only the elements matching the selection of the source. The match is done by
 * index, not by value, so in theory they should have the same options.
 */
function selectSameIndexes(srcComp, trgComp) {
  if (srcComp.options && trgComp.options) {
    var size = Math.min(srcComp.options.length, trgComp.options.length);
    for (var i = 0; i < size; i++) {
      trgComp.options[i].selected = srcComp.options[i].selected;
    }
    for (var i = size; i < trgComp.options.length; i++) {
      trgComp.options[i].selected = false;
    }
  }
}

/**
 * Adds preselected Radio Buttons to currentRadioSelectedByGroupMap to make
 * behaviors work
 */
function initRadioModel() {
  jQuery("input[type=radio][checked=yes][name]").each(function(index, node) {
    currentRadioSelectedByGroupMap[currentRadioSelectedByGroupMap.length] = {
      groupName : node.getAttribute("name"),
      radioId : node.getAttribute("id")
    };
  });
}

/**
 * Execute behaviors once when opening the form (the components that contain
 * behaviors are marked with the css class "cbh").
 */
function exeBehFirstTime() {
  jQuery(".cbh").each(
      function() {
        var type = this.type;
        if (type === "select-one" || type === "select-multiple"
            || type === "textarea" || type === "text") {
          jQuery(this).trigger("change");
        } else {
          var checkedValue = (this).checked;
          if (type === "radio" && checkedValue) {
            jQuery(this).trigger("click");
            this.checked = checkedValue;
          } else if (type === "checkbox") {
            this.checked = !checkedValue;
            jQuery(this).trigger("click");
            this.checked = checkedValue;
          }
        }
      });
}

/**
 * Hide the "select_record_item_button" button in the case that the form is not
 * dynamic or the record item iterator is marked as "Random" in those cases this
 * button is not necessary.
 */
function hideSelectRIButton() {
  if (removeSelectRIButton) {
    jQuery(".btn_show_select_record_items_class").each(function() {
      jQuery(this).hide();
    });
  }
}

/**
 * Shows the "Select Record Item" modal dialog that is used to select which
 * selectable record items should be shown/hidden on a dynamic form.
 * 
 * @returns {Boolean} Always false.
 */
function showSelectRecordItemModal() {
  // Check if there are selectable event record items at all. This should
  // actually not happen because the button is
  // actually hidden when there are not selectable event record items, but safe
  // is safe.
  var hiddenInputs = jQuery('input:hidden[id*=selectable_record_items_]');
  var $recordItemModal = jQuery('#select_recorditems_modal');
  var $modalBody = $recordItemModal.find('.modal-body');
  var $modalFooter = $recordItemModal.find('.modal-footer');
  var dismissButton = jQuery('<button>Cancel</button>').attr({
    'data-dismiss' : "modal",
    'type' : "button",
    'class' : "btn btn-default"
  });

  if (hiddenInputs.length <= 0 || !hiddenInputs[0].value) {
    var $message = jQuery('<p>This form does not have any selectable event record items! Please check the setup in the project event.</p>');
    $modalBody.empty().append($message);
    $modalFooter.empty().append(dismissButton);
    $recordItemModal.modal('show');
  } else {

    var firstInput = hiddenInputs[0];

    // Remove all contents of table
    var $recordItemsTable = jQuery('#select_recorditems_modal_table');
    $recordItemsTable.empty();

    // Add table header with "select all" check box
    var $tableHeader = jQuery('<thead></thead>');
    var $tableRow = jQuery('<tr></tr>');
    var $tableHead = jQuery('<th></th>');
    var $checkbox = jQuery('<input/>').attr({
      'id' : 'chk_select_all',
      'type' : 'checkbox'
    });

    var $checkboxHeader = $tableHead.clone().attr({
      'width' : '2em'
    }).append($checkbox);

    var $recordItemHeader = $tableHead.clone().append('Record Item');

    var $eventNameheader = $tableHead.clone().append('Event Name');

    $recordItemsTable.append($tableHeader.append($tableRow.append(
        $checkboxHeader).append($recordItemHeader).append($eventNameheader)));

    $checkbox.on('change', function() {
      var checked = this.checked;
      jQuery("input:checkbox[name*=selectable_checkbox]").prop("checked",
          checked);
    });

    var allSelected = true;

    // Get JSON string from hidden input containing record item data and
    // selected
    // state
    var eris = JSON.parse(firstInput.value);
    for (var i = 0; i < eris.event_record_items.length; i++) {
      var eri = eris.event_record_items[i];

      allSelected = allSelected && eri.selected;

      jQuery(
          '<tr><td><input type="checkbox" '.concat(
              eri.selected ? 'checked="checked" ' : "").concat('value="')
              .concat(eri.event_record_item_key).concat("~").concat(
                  eri.record_item_name).concat("~").concat(eri.event_name)
              .concat('" name="selectable_checkbox_').concat(
                  eri.event_record_item_key).concat('"/></td><td>').concat(
                  eri.record_item_name).concat('</td><td>').concat(
                  eri.event_name).concat('</td></tr>')).appendTo(
          '#select_recorditems_modal_table');
    }

    // If all record items are shown (=selected), make the "select all" check
    // box
    // also checked
    if (allSelected) {
      jQuery('#chk_select_all').prop("checked", "checked");
    }

    // Update "select all" checkbox according to selected record items
    jQuery("input:checkbox[name*=selectable_checkbox]").change(function() {
      var allChecked = true;
      var checkboxes = jQuery("input:checkbox[name*=selectable_checkbox]");
      for (var i = 0; i < checkboxes.length; i++) {
        allChecked = allChecked && checkboxes[i].checked;
      }

      jQuery('#chk_select_all').prop("checked", allChecked);
    });

    // Show modal
    $recordItemModal.modal('show');
    return false;
  }
}

/**
 * Closes the "Select Record Items" modal dialog and saves the currently
 * selected data to the hidden input.
 */
function closeShowSelectRecordItemModal() {
  // Hide modal
  jQuery('#select_recorditems_modal').modal('hide');

  // Build object containing the data from the checkboxes
  var data = [];
  var checkboxes = jQuery("input:checkbox[name*=selectable_checkbox]");
  for (var i = 0; i < checkboxes.length; i++) {
    var chk = checkboxes[i];
    var values = chk.value.split("~");
    data.push({
      "event_record_item_key" : values[0],
      "event_name" : values[2],
      "record_item_name" : values[1],
      "selected" : chk.checked
    });
  }
  var finalData = {
    "event_record_items" : data
  };

  // Put JSON representation of object into hidden input
  jQuery('input:hidden[id*=selectable_record_items_]').prop("value",
      JSON.stringify(finalData));

  // shows and hide the specified event record Item
  processToHideAndShowRII();
}

/* TAKEN FROM OLD formBuilder.js file, as they could be useful in the future * */

// shows if mandatory is evaluated in the form
var mandatory = null;

function setMandatory(value) {
  this.mandatory = value;
}

/** ************************************************************************************************** */

/**
 * global variable containing data for show and hide event record Item iterator
 */

var GLOBAL_OBJ = {};

/**
 * UTIL METHOD This function allows to know if the specified strId string
 * parameter contains inside it some key from keys array parameter
 */
function isERIKInId(keys, strId) {
  if (keys && strId) {
    for ( var i in keys) {
      if (strId.indexOf("ERIK_" + keys[i]) >= 0) {
        return true;
      }
    }
  }
  return false;
}

/**
 * UTIL METHOD get all event record Items keys from hidden input applied the
 * filter passed as parameter. The filter is a predicate, it takes an
 * eventRecordItem wrapper Object constructed above this method. for an example
 * take a look to closeShowSelectRecordItemModal() method to see how is built
 * this object.
 */
function getEventRIKey(filter) {
  var hiddenInputs = jQuery('input:hidden[id*=selectable_record_items_]');
  var erisToReturn = [];
  if (hiddenInputs.length >= 1 && hiddenInputs[0].value) {
    var eriWrapper = JSON.parse(hiddenInputs[0].value);
    for (var i = 0; i < eriWrapper.event_record_items.length; i++) {
      if (filter(eriWrapper.event_record_items[i])) {
        erisToReturn
            .push(eriWrapper.event_record_items[i].event_record_item_key);
      }
    }
  }
  return erisToReturn;
}

/**
 * UTIL METHOD get all below siblings of specified node, node must be JQuery
 * object.
 */

function getSblingsBelow($node) {

  if ($node.position() !== undefined) {
    var Xend = $node.position().top + $node.height();
    var $siblings = $node.siblings("div");
    var $siblingsToReturn = [];
    $siblings.each(function() {
      var Ystart = jQuery(this).position().top;
      if (Xend < Ystart) {
        jQuery(this).prop("Ystart", Ystart);
        $siblingsToReturn.push(jQuery(this));
      }
    });

  }

  return $siblingsToReturn;
}

/**
 * UTIL METHOD Set the specified height for all siblings nodes. siblings
 * parameter is an array of jQuery objects.
 */
function updateSiblingNodes(siblings, heigth) {
  if (siblings.length > 0 && heigth !== 0) {
    for (var i = 0; i < siblings.length; i++) {
      var $node = siblings[i];
      $node.prop("moved", true);
      var top = $node.position().top;
      $node.css({
        "top" : (top + heigth) + "px"
      });
    }
  }
}

/**
 * UTIL METHOD
 */
function getAllMovedElements() {
  var $objToReturn = [];
  var $SIBLINGS_BELOW = GLOBAL_OBJ["$SIBLINGS_BELOW"];
  if (!($SIBLINGS_BELOW)) {
    return $objToReturn;
  }
  for (var i = 0; i < $SIBLINGS_BELOW.length; i++) {
    var $node = $SIBLINGS_BELOW[i];
    if ($node.prop("moved")) {
      $objToReturn.push($node);
    }
  }
  var riNumber = GLOBAL_OBJ["RIITERATOR_NUMBER"];
  for (var j = 0; j < riNumber; j++) {
    var RIITERATOR_x = GLOBAL_OBJ["RIITERATOR_" + j];
    var $SIBLINGS_BELOW = RIITERATOR_x["$SIBLINGS_BELOW"];
    for (var z = 0; z < $SIBLINGS_BELOW.length; z++) {
      var $node = $SIBLINGS_BELOW[z];
      if ($node.prop("moved")) {
        $objToReturn.push($node);
      }
    }
  }
  return $objToReturn;
}

/**
 * UTIL METHOD
 */
function getTopMovedObject($listObj, str) {
  if ($listObj && $listObj.length > 0 && str) {
    for (var i = 0; i < $listObj.length; i++) {
      var $node = $listObj[i];
      if ($node.attr("id") === str) {
        return $node.prop("Ystart");
      }
    }
  }
  return "NotFound";
}

/**
 * event handler when user confirm Select Record Item dialog.
 */
function processToHideAndShowRII() {
  if (GLOBAL_OBJ["SELECTABLESRIKEY"].length > 0
      && GLOBAL_OBJ["RIITERATOR_NUMBER"] > 0) {
    var elementsToShow = getEventRIKey(function(eri) {
      return eri && eri.selected;
    });

    var riNumber = GLOBAL_OBJ["RIITERATOR_NUMBER"];
    for (var i = 0; i < riNumber; i++) {
      var RIITERATOR_x = GLOBAL_OBJ["RIITERATOR_" + i];
      evaluateToShowAndHide(RIITERATOR_x, elementsToShow);
      updateSiblingNodesForRII(RIITERATOR_x);
      showAndHideElements(RIITERATOR_x);
      updateSignoffForIterator(RIITERATOR_x);
    }

    updateSiblingNodesForSFI();
    updateFormComponent();
  }
}

/**
 * update height for FormComponent object inside form.
 */
function updateFormComponent() {
  var $FORMCOMPONENT = GLOBAL_OBJ["$FORMCOMPONENT"];
  var heigth = getHeightForAllRIIs();
  if (heigth !== 0) {
    $FORMCOMPONENT.height($FORMCOMPONENT.height() + heigth);
  }
}

/**
 * update sibling Nodes to signoff for iterator containing the record item
 * iterator.
 */
function updateSiblingNodesForSFI() {
  var siblings = GLOBAL_OBJ["$SIBLINGS_BELOW"];
  var heigth = getHeightForAllRIIs();
  if (siblings.length > 0 && heigth !== 0) {
    updateSiblingNodes(siblings, heigth);
  }
}

/**
 * get HEIGHT_TO_ADD property For All RIITERATOR_x objects inside in GLOBAL_OBJ
 * object.
 */
function getHeightForAllRIIs() {
  var riNumber = GLOBAL_OBJ["RIITERATOR_NUMBER"];
  var heigth = 0;
  for (var i = 0; i < riNumber; i++) {
    var RIITERATOR_x = GLOBAL_OBJ["RIITERATOR_" + i];
    heigth += RIITERATOR_x["HEIGHT_TO_ADD"];
  }
  return heigth;
}

/**
 * update each Signoff For Iterator parent for RIITERATOR_x object.
 */
function updateSignoffForIterator(RIITERATOR_x) {
  var heigth = RIITERATOR_x["HEIGHT_TO_ADD"];
  if (heigth !== 0) {
    var $rii = RIITERATOR_x["$RIITERATOR"];
    var $signoffParent = $rii.parent("div[id^='FB_ITERATOR']");
    $signoffParent.height($signoffParent.height() + heigth);
  }
}

/**
 * update sibling Nodes For the RIITERATOR_x object
 */
function updateSiblingNodesForRII(RIITERATOR_x) {
  var siblings = RIITERATOR_x["$SIBLINGS_BELOW"];
  var heigth = RIITERATOR_x["HEIGHT_TO_ADD"];
  if (siblings.length > 0 && heigth !== 0) {
    updateSiblingNodes(siblings, heigth);
  }
}

/**
 * show and hide elements to RIITERATOR_x parameter.
 */
function showAndHideElements(RIITERATOR_x) {
  var riToShow = RIITERATOR_x.$SELECTABLESRI_TO_SHOW;
  var riToHide = RIITERATOR_x.$SELECTABLESRI_TO_HIDE;
  for (var s = 0; s < riToShow.length; s++) {
    riToShow[s].css({
      "display" : "block",
      "visibility" : "visible"
    });
  }
  for (var h = 0; h < riToHide.length; h++) {
    riToHide[h].css({
      "display" : "none",
      "visibility" : "hidden"
    });
  }
}

/**
 * evaluate what elements should be shown and hidden for RIITERATOR_x parameter.
 */
function evaluateToShowAndHide(RIITERATOR_x, elementsToShow) {
  RIITERATOR_x["$SELECTABLESRI_TO_SHOW"] = [];
  RIITERATOR_x["$SELECTABLESRI_TO_HIDE"] = [];
  RIITERATOR_x["HEIGHT_TO_ADD"] = 0;
  var $SELECTABLESRI = RIITERATOR_x["$SELECTABLESRI"];
  for (var j = 0; j < $SELECTABLESRI.length; j++) {
    var $rii = $SELECTABLESRI[j];
    var elementId = $rii.attr("id");
    if (isERIKInId(elementsToShow, elementId)) {
      if ($rii.css("display") !== "block") {
        RIITERATOR_x["HEIGHT_TO_ADD"] += $rii.height();
        RIITERATOR_x["$SELECTABLESRI_TO_SHOW"].push($rii);
      }
    } else {
      if ($rii.css("display") !== "none") {
        RIITERATOR_x["HEIGHT_TO_ADD"] -= $rii.height();
        RIITERATOR_x["$SELECTABLESRI_TO_HIDE"].push($rii);
      }
    }
  }
}

/**
 * Initialize the process to show and hide Record item iterators Below is the
 * object description representing all information used to manage the show and
 * hide record item iterator process { SELECTABLESRIKEY:[keys], $FORMCOMPONENT:
 * jquery_obj, $SFITERATOR: jquery_obj, $SIBLINGS_BELOW:[jquery_obj],//
 * $SFITERATOR siblings below RIITERATOR_NUMBER: number,// One for each trainee
 * or item selected RIITERATOR_0:{ $RIITERATOR:jquery_obj,//container
 * $SIBLINGS_BELOW:[jquery_obj],// $RIITERATOR siblings below
 * $ALLRI:[jquery_obj],//all child iterators $SELECTABLESRI:[jquery_obj],//child
 * iterators $SELECTABLESRI_TO_SHOW:[jquery_obj],// initially is void
 * $SELECTABLESRI_TO_HIDE:[jquery_obj],//initially is void
 * HEIGHT_TO_ADD:number//initially 0 }, RIITERATOR_1:{//similar to above },
 * RIITERATOR_x:{//similar to above } }
 */
function initProcessToHideAndShowRII() {
  GLOBAL_OBJ["SELECTABLESRIKEY"] = getEventRIKey(function() {
    return true;
  });

  var $FORMCOMPONENT = jQuery("div.formComponent[id='divContentForm']");
  GLOBAL_OBJ["$FORMCOMPONENT"] = $FORMCOMPONENT;

  var $SFITERATOR = jQuery("div[componenttype=RIITERATOR]").closest(
      "div[componenttype=SFITERATOR]");
  GLOBAL_OBJ["$SFITERATOR"] = $SFITERATOR;

  GLOBAL_OBJ["$SIBLINGS_BELOW"] = getSblingsBelow($SFITERATOR);

  var $RIITERATOR = $SFITERATOR.find("div[componenttype='RIITERATOR']");
  GLOBAL_OBJ["RIITERATOR_NUMBER"] = $RIITERATOR.length;

  $RIITERATOR.each(function(index) {
    var $iterator = jQuery(this);
    var obj = {};

    obj["$RIITERATOR"] = $iterator;

    obj["$SIBLINGS_BELOW"] = getSblingsBelow($iterator);

    obj["$SELECTABLESRI"] = [];
    obj["$ALLRI"] = [];
    $iterator.find("div[id^='FB_ITERATOR']").each(function() {
      obj["$ALLRI"].push(jQuery(this));
      if (isERIKInId(GLOBAL_OBJ["SELECTABLESRIKEY"], jQuery(this).attr("id"))) {
        obj["$SELECTABLESRI"].push(jQuery(this));
      }
    });

    obj["$SELECTABLESRI_TO_SHOW"] = [];
    obj["$SELECTABLESRI_TO_HIDE"] = [];
    obj["HEIGHT_TO_ADD"] = 0;

    GLOBAL_OBJ["RIITERATOR_" + index] = obj;
  });

  processToHideAndShowRII();
}

// This code will prevent that pressing enter in the form presses any button
jQuery(document).ready(function() {
  jQuery('#form').on('keyup keypress', function(e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13 && !$(document.activeElement).is('textarea')) { 
      e.preventDefault();
      return false;
    }
  });
});

/** ************************************************************************************************** */

/* End section --- Please place new methods on top of this section!!! * */
