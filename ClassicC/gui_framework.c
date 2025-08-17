#include "gui_framework.h"

// Create a window
Window* createWindow(int width, int height, const char* title) {
    Window* win = malloc(sizeof(Window));
    win->width = width;
    win->height = height;
    strncpy(win->title, title, sizeof(win->title));
    win->widgets = NULL;
    return win;
}

// Create a button widget
Widget* createButton(int x, int y, int width, int height, const char* text) {
    Widget* w = malloc(sizeof(Widget));
    w->x = x; w->y = y; w->width = width; w->height = height;
    w->type = WIDGET_BUTTON;
    strncpy(w->text, text, sizeof(w->text));
    w->onClick = NULL;
    w->next = NULL;
    return w;
}

// Create a label widget
Widget* createLabel(int x, int y, const char* text) {
    Widget* w = malloc(sizeof(Widget));
    w->x = x; w->y = y; w->width = 0; w->height = 0;
    w->type = WIDGET_LABEL;
    strncpy(w->text, text, sizeof(w->text));
    w->onClick = NULL;
    w->next = NULL;
    return w;
}

// Add widget to window
void addWidget(Window* window, Widget* widget) {
    widget->next = window->widgets;
    window->widgets = widget;
}

// Render window and widgets (stub)
void renderWindow(Window* window) {
    printf("Window: %s (%dx%d)\n", window->title, window->width, window->height);
    Widget* w = window->widgets;
    while (w) {
        printf("  Widget: %s at (%d,%d)\n", w->text, w->x, w->y);
        w = w->next;
    }
}

// Handle input (stub: simulate button click)
void handleInput(Window* window) {
    Widget* w = window->widgets;
    while (w) {
        if (w->type == WIDGET_BUTTON && w->onClick) {
            printf("Simulating click on button: %s\n", w->text);
            w->onClick(w);
        }
        w = w->next;
    }
}