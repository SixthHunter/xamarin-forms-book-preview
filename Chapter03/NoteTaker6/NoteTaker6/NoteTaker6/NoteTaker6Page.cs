﻿using System;
using Xamarin.Forms;

namespace NoteTaker6
{
    class NoteTaker6Page : ContentPage
    {
        static readonly string FILENAME = "test.note";

        public NoteTaker6Page()
        {
            // Create Entry and Editor views.
            Entry entry = new Entry
            {
                Placeholder = "Title (optional)"
            };

            Editor editor = new Editor
            {
                Keyboard = Keyboard.Create(KeyboardFlags.All),
                BackgroundColor = Device.OnPlatform(Color.Default, 
                                                    Color.Default, 
                                                    Color.White),
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            // Set data bindings.
            Note note = new Note();
            this.BindingContext = note;
            entry.SetBinding(Entry.TextProperty, "Title");
            editor.SetBinding(Editor.TextProperty, "Text");

            // Create Save and Load buttons.
            Button saveButton = new Button
            {
                Text = "Save",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Button loadButton = new Button
            {
                Text = "Load",
                IsEnabled = false,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            // Set Clicked handlers.
            saveButton.Clicked += async (sender, args) =>
                {
                    saveButton.IsEnabled = false;
                    loadButton.IsEnabled = false;
                    await note.SaveAsync(FILENAME);
                    saveButton.IsEnabled = true;
                    loadButton.IsEnabled = true;
                };

            loadButton.Clicked += async (sender, args) =>
                {
                    saveButton.IsEnabled = false;
                    loadButton.IsEnabled = false;
                    await note.LoadAsync(FILENAME);
                    saveButton.IsEnabled = true;
                    loadButton.IsEnabled = true;
                };

            // Check if the file is available.
            FileHelper.ExistsAsync(FILENAME).ContinueWith((task) =>
                {
                    loadButton.IsEnabled = task.Result;
                });

            // Assemble page.
            this.Padding =
                new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 0);

            this.Content = new StackLayout
            {
                Children = 
                {
                    new Label 
                    { 
                        Text = "Title:" 
                    },
                    entry,
                    new Label 
                    { 
                        Text = "Note:" 
                    },
                    editor,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = 
                        {
                            saveButton,
                            loadButton
                        }
                    }
                }
            };
        }
    }
}
