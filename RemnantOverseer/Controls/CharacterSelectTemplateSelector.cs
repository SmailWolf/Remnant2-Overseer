using Avalonia.Controls.Templates;
using Avalonia.Controls;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;
using RemnantOverseer.Models;

// Why?
// https://stackoverflow.com/questions/74003533/avalonia-treeview-template-selector
// What?
// https://github.com/AvaloniaUI/Avalonia.Samples/tree/main/src/Avalonia.Samples/DataTemplates/IDataTemplateSample

namespace RemnantOverseer.Controls;
internal class CharacterSelectTemplateSelector : IDataTemplate
{
    [Content]
    public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = [];

    // Check if we can accept the provided data
    public bool Match(object? data)
    {
        return data is Character;
    }

    // Build the DataTemplate here
    Control? ITemplate<object?, Control?>.Build(object? param)
    {
        if (param == null) return null;
        string key = GetKey(param);
        return AvailableTemplates[key].Build(param);
    }

    private string GetKey(object obj)
    {
        if (obj is Character character)
        {
            return character switch
            {
                { SubArchetype: null } => "OneArchetypeTemplate",
                { SubArchetype: not null } => "TwoArchetypeTemplate",
            };
        }
        else throw new ArgumentException("Unknown type in passed to the selector");
    }
}