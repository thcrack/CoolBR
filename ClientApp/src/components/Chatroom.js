import React, { Component } from 'react';
import Card from "reactstrap/es/Card";
import * as SignalR from '@microsoft/signalr';
import authService from './api-authorization/AuthorizeService'
import './Chatroom.css'

export class Chatroom extends Component {
    static displayName = Chatroom.name;
    
    constructor(props) {
        super(props);
        this.state = { messages: [], loading: true, messageInput: "", chatroomReady: false};
        this.handleSubmit = this.handleSubmit.bind(this);
        this.messageInputChanged = this.messageInputChanged.bind(this);
    }
    
    componentDidMount() {
        this.connection = new SignalR.HubConnectionBuilder()
            .withUrl("/hub/chat").build();
        this.connection.on("ReceiveMessage", 
            (user, message) => {
            let msgArea = document.getElementById('chatroom-messages');
            let wasAtBottom = msgArea.scrollTop === msgArea.scrollTopMax;
            this.setState({
                messages: [...this.state.messages, {
                    "user": user,
                    "content": message
                }]
            }, () => {
                if(wasAtBottom){
                    msgArea.scrollTop = msgArea.scrollTopMax;
                }
            });
        });
        this.connection.start().then(() => {
            this.setState({chatroomReady: true});
        });
        this.populateMessages();
    }

    renderChatroom(messages) {
        return (
            <div className="chatroom">
                <h2>Chatroom</h2>
                <div className={Card} id="chatroom-form">
                    <form onSubmit={this.handleSubmit}>
                        <div className="form-group">
                            <input type="text" className="form-control" id="message-input"
                                   onChange={this.messageInputChanged}
                                   value={this.state.messageInput}
                                   placeholder="Enter your message..."
                            />
                        </div>
                        <button type="submit" className="btn btn-primary" disabled={ !this.state.chatroomReady }>
                            {this.state.chatroomReady ? "Submit" : "Connecting..."}
                        </button>
                    </form>
                </div>
                <div className={Card} id="chatroom-messages">
                    {messages.map(msg =>
                        <p><b>{msg.user}</b>: {msg.content}</p>
                    )}
                </div>
            </div>
        );
    }

    render() {
        return (
            this.state.loading
                ? <p><em>Loading...</em></p>
                : this.renderChatroom(this.state.messages)
        );
    }

    async handleSubmit(e) {
        e.preventDefault();
        let msg = this.state.messageInput.trim();
        if(msg === "") return;
        const user = await authService.getUser();
        await this.connection.invoke("SendMessage", user ? user.name : "Anonymous", msg);
        this.setState({
            messageInput: ""
        });
    }

    populateMessages() {
        this.setState({
            loading: false
        });
    }

    messageInputChanged(e) {
        this.setState({
            messageInput: e.target.value
        });
    }
}