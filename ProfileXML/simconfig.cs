﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
[System.Xml.Serialization.XmlRootAttribute("SimConfig", Namespace="http://www.pilborough.co.uk/sim/config", IsNullable=false)]
public partial class ProfileList {
    
    private Config configField;
    
    private TSLoco[] tsMappingField;
    
    private Profile[] profileField;
    
    /// <remarks/>
    public Config config {
        get {
            return this.configField;
        }
        set {
            this.configField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("loco", IsNullable=false)]
    public TSLoco[] tsMapping {
        get {
            return this.tsMappingField;
        }
        set {
            this.tsMappingField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("profile")]
    public Profile[] profile {
        get {
            return this.profileField;
        }
        set {
            this.profileField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class Config {
    
    private ConfigValue[] valueField;
    
    private string brokerAddressField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Value")]
    public ConfigValue[] Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
    
    /// <remarks/>
    public string brokerAddress {
        get {
            return this.brokerAddressField;
        }
        set {
            this.brokerAddressField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class ConfigValue {
    
    private string idField;
    
    private string valueField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ID {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class KeyPressSequence {
    
    private KeyPressAction[] stepField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("step")]
    public KeyPressAction[] step {
        get {
            return this.stepField;
        }
        set {
            this.stepField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class KeyPressAction {
    
    private bool shiftField;
    
    private bool shiftFieldSpecified;
    
    private bool ctrlField;
    
    private bool ctrlFieldSpecified;
    
    private bool altField;
    
    private bool altFieldSpecified;
    
    private string valueField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool shift {
        get {
            return this.shiftField;
        }
        set {
            this.shiftField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool shiftSpecified {
        get {
            return this.shiftFieldSpecified;
        }
        set {
            this.shiftFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool ctrl {
        get {
            return this.ctrlField;
        }
        set {
            this.ctrlField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ctrlSpecified {
        get {
            return this.ctrlFieldSpecified;
        }
        set {
            this.ctrlFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool alt {
        get {
            return this.altField;
        }
        set {
            this.altField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool altSpecified {
        get {
            return this.altFieldSpecified;
        }
        set {
            this.altFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class KeyPressToggle {
    
    private KeyPressAction activateField;
    
    private KeyPressAction deactivateField;
    
    /// <remarks/>
    public KeyPressAction activate {
        get {
            return this.activateField;
        }
        set {
            this.activateField = value;
        }
    }
    
    /// <remarks/>
    public KeyPressAction deactivate {
        get {
            return this.deactivateField;
        }
        set {
            this.deactivateField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class KeyPressFunction {
    
    private string topicField;
    
    private object[] itemsField;
    
    /// <remarks/>
    public string topic {
        get {
            return this.topicField;
        }
        set {
            this.topicField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("hold", typeof(KeyPressAction))]
    [System.Xml.Serialization.XmlElementAttribute("sequence", typeof(KeyPressSequence))]
    [System.Xml.Serialization.XmlElementAttribute("toggle", typeof(KeyPressToggle))]
    public object[] Items {
        get {
            return this.itemsField;
        }
        set {
            this.itemsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class FirmataPin {
    
    private string[] topicField;
    
    private int idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("topic")]
    public string[] topic {
        get {
            return this.topicField;
        }
        set {
            this.topicField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class KeyPosition {
    
    private LightState blueField;
    
    private LightState redField;
    
    /// <remarks/>
    public LightState blue {
        get {
            return this.blueField;
        }
        set {
            this.blueField = value;
        }
    }
    
    /// <remarks/>
    public LightState red {
        get {
            return this.redField;
        }
        set {
            this.redField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public enum LightState {
    
    /// <remarks/>
    on,
    
    /// <remarks/>
    off,
    
    /// <remarks/>
    blink,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class Key {
    
    private KeyType typeField;
    
    private string topicField;
    
    private KeyPosition onField;
    
    private KeyPosition offField;
    
    private int idField;
    
    /// <remarks/>
    public KeyType type {
        get {
            return this.typeField;
        }
        set {
            this.typeField = value;
        }
    }
    
    /// <remarks/>
    public string topic {
        get {
            return this.topicField;
        }
        set {
            this.topicField = value;
        }
    }
    
    /// <remarks/>
    public KeyPosition on {
        get {
            return this.onField;
        }
        set {
            this.onField = value;
        }
    }
    
    /// <remarks/>
    public KeyPosition off {
        get {
            return this.offField;
        }
        set {
            this.offField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int ID {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public enum KeyType {
    
    /// <remarks/>
    normal,
    
    /// <remarks/>
    toggle,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class Mapping {
    
    private Key[] xKeysInputField;
    
    private FirmataPin[] firmataInputField;
    
    private KeyPressFunction[] keypressOutputField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
    public Key[] XKeysInput {
        get {
            return this.xKeysInputField;
        }
        set {
            this.xKeysInputField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("pin", IsNullable=false)]
    public FirmataPin[] firmataInput {
        get {
            return this.firmataInputField;
        }
        set {
            this.firmataInputField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("keypress", IsNullable=false)]
    public KeyPressFunction[] keypressOutput {
        get {
            return this.keypressOutputField;
        }
        set {
            this.keypressOutputField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class Profile {
    
    private Mapping mappingsField;
    
    private string nameField;
    
    /// <remarks/>
    public Mapping Mappings {
        get {
            return this.mappingsField;
        }
        set {
            this.mappingsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(TSCustomControl))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class TSControl {
    
    private string variableField;
    
    private string notchesField;
    
    private float stepField;
    
    private bool stepFieldSpecified;
    
    private float centerDetentField;
    
    private bool centerDetentFieldSpecified;
    
    /// <remarks/>
    public string variable {
        get {
            return this.variableField;
        }
        set {
            this.variableField = value;
        }
    }
    
    /// <remarks/>
    public string notches {
        get {
            return this.notchesField;
        }
        set {
            this.notchesField = value;
        }
    }
    
    /// <remarks/>
    public float step {
        get {
            return this.stepField;
        }
        set {
            this.stepField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool stepSpecified {
        get {
            return this.stepFieldSpecified;
        }
        set {
            this.stepFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public float centerDetent {
        get {
            return this.centerDetentField;
        }
        set {
            this.centerDetentField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool centerDetentSpecified {
        get {
            return this.centerDetentFieldSpecified;
        }
        set {
            this.centerDetentFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class TSCustomControl : TSControl {
    
    private string topicField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string topic {
        get {
            return this.topicField;
        }
        set {
            this.topicField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pilborough.co.uk/sim/config")]
public partial class TSLoco {
    
    private TSControl throttleField;
    
    private TSCustomControl[] customField;
    
    private string nameField;
    
    /// <remarks/>
    public TSControl throttle {
        get {
            return this.throttleField;
        }
        set {
            this.throttleField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("custom")]
    public TSCustomControl[] custom {
        get {
            return this.customField;
        }
        set {
            this.customField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
}
