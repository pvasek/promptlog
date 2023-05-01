# promptlog

Simple UI for your OpenAI prompts. 

It's just a proxy server to the OpenAI API endpoint that logs all the prompts and responses.
The headers are not stored which means it doesn't store your OpenAI API key. 

## Limitations
Only _chat_ API endpoint is supported (the server logs everything but UI was tested/prepared only for _chat_ endpoint).
Only SQL Server is supported right now.

## Configuration

To prepare your deployment you need to configure by using system environment variables:

- `PromptLog__ConnectionString` - the connection string to SQL Server database.
- `PromptLog__OpenAIApiBase` - the endpoint to your OpenAI API. Default: `https://api.openai.com/`

## Local development

For the configuration, you can use `appsettings.Development.json` file or `user-secrets`. In this case, the configuration keys are the following:
- `PromptLog:ConnectionString` - the connection string to SQL Server database.
- `PromptLog:OpenAIApiBase` - the endpoint to your OpenAI API. Default: `https://api.openai.com/`