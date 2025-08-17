#include "gui_framework.h"

// Callback functions
void onHelloClick(Widget* widget) {
    printf("Hello button clicked! Output: Welcome to the GUI framework!\n");
}

void onTestClick(Widget* widget) {
    printf("Test button clicked! Output: All systems working perfectly!\n");
}

int main(void) {
    // Create main window
    Window* mainWindow = createWindow(50, 10, "Test Project GUI");
    
    // Create widgets
    Widget* helloButton = createButton(5, 3, 10, 2, "Hello");
    Widget* testButton = createButton(5, 5, 10, 2, "Test");
    Widget* titleLabel = createLabel(5, 1, "Custom GUI Framework Demo");
    
    // Set button callbacks
    helloButton->onClick = onHelloClick;
    testButton->onClick = onTestClick;
    
    // Add widgets to window
    addWidget(mainWindow, titleLabel);
    addWidget(mainWindow, helloButton);
    addWidget(mainWindow, testButton);
    
    // Main application loop
    while (1) {
        renderWindow(mainWindow);
        handleInput(mainWindow);
    }
    
    return 0;
}