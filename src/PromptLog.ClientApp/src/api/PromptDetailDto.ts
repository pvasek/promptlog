export interface PromptDetailDto {
    id: string;
    created: string;
    request: ChatRequest;
    response: ChatResponse;
    raiting: number;
    url: string;
}

export interface ChatRequestMessage {
    role: string;
    content: string;
}

export interface ChatRequest {
    messages: ChatRequestMessage[];
    model: string;
    temperature: number;
    max_tokens: number;
}

export interface ChatResponseChoice {
    index: number;
    finish_reason: string;
    message: ChatRequestMessage;
}

export interface ChatResponseUsage {
    completion_tokens: number;
    prompt_tokens: number;
    total_tokens: number;
}

export interface ChatResponse {
    id: string;
    object: string;
    created: string;
    model: string;
    choices: ChatResponseChoice[];
    usage: ChatResponseUsage;
}



