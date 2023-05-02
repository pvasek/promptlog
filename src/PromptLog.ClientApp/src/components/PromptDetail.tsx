import React, { Component } from 'react';
import { FaHeart, FaRegHeart, FaCube, FaUser, FaRobot } from 'react-icons/fa'
import { PromptDetailDto } from "../api/PromptDetailDto";
import { Raiting } from "./Raiting";

interface PromptDetailProps {
    prompt: PromptDetailDto;
    onRaitingClick: (prompt: PromptDetailDto) => void;
}

export class PromptDetail extends Component<PromptDetailProps> {
    static displayName = PromptDetail.name;

    constructor(props: PromptDetailProps) {
        super(props);
        this.state = { prompt: null };
    }

    render() {
        if (!this.props.prompt) {
            return (<div className="prompt-detail">Select a prompt</div>);
        }
        return (
            <div className="prompt-detail">
                <div className="prompt-header">
                    <div className="created">{this.props.prompt.created}</div>
                    <div className="model">model: {this.props.prompt.request.model}</div>
                    <div className="model">temperature: {this.props.prompt.request.temperature}</div>
                    <Raiting
                        raiting={this.props.prompt.raiting}
                        onClick={() => this.props.onRaitingClick(this.props.prompt)}
                    />
                </div>
                <div className="prompt-content">
                    <label>PROMPT</label>
                    <div>{this.props.prompt.request.messages?.map((i, idx) => (
                        <div key={idx} className="choice">
                            {i.role == "system" && <FaCube />}
                            {i.role == "user" && <FaUser />}
                            {i.role == "assistant" && <FaRobot />}
                            <pre className={i.role}>{i.content}</pre>
                        </div>
                    ))}</div>
                    <label>RESPONSE</label>
                    <div>{this.props.prompt.response.choices?.map((i, idx) => (
                        <div key={idx} className="choice">                            
                            {i.message.role == "system" && <FaCube />}
                            {i.message.role == "user" && <FaUser />}
                            {i.message.role == "assistant" && <FaRobot />}
                            <pre className={`message ${i.message.role}`}>{i.message.content}</pre>
                        </div>
                    ))}</div>
                </div>
            </div>
        );
    }

}
