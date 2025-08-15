#include<stdio.h>
#include<string.h>

int main(void)
{   
    char input[100];
    char name[20];

    printf("Enter a command (hello, goodbye, test): \n");
    fgets(input, sizeof(input), stdin);

    printf("What is your name: ");
    fgets(name, sizeof(name), stdin);

    input[strcspn(input, "\n")] = 0;
    name[strcspn(name, "\n")] = 0;

    if(strcmp(input, "hello")==0){
        printf("Hello %s! Welcome to the test project.\n", name);
    } else if(strcmp(input, "goodbye") == 0){
        printf("Goodbye! Thanks for using the test project.\n");
    } else if (strcmp(input, "test") == 0){
        printf("Running test... All systems operational!\n");
    } else {
        printf("Unknown command: %s\n", input);
    }

    //printf("hello world\n");
    
    return 0;
}    
