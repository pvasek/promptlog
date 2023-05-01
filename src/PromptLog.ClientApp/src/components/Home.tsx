import React, { Component } from 'react';
import { PromptList } from "./PromptList";
import { PromptDetail } from "./PromptDetail";
import { PromptDetailDto } from "../api/PromptDetailDto";

interface HomeState {
    items: any[];
    loading: boolean;
    selectedItem: any;
    raitingFilter: number;
}

interface HomeProps { }

export class Home extends Component<HomeProps, HomeState> {
    static displayName = Home.name;

    constructor(props: HomeProps) {
        super(props);
        this.state = { 
            items: [], 
            loading: true, 
            selectedItem: null,
            raitingFilter: 0,
        };
    }

    componentDidMount() {
        document.onkeydown = e => this.onKeyDown(e);
        this.fetchData();
    }

    render() {
        if (this.state.loading) {
            return (<p><em>Loading...</em></p>);
        }

        return (
            <div className="prompt-container">
                <PromptList items={this.state.items} 
                    onSelected={i => this.onItemSelected(i)} 
                    selectedItem={this.state.selectedItem}
                    raitingFilter={this.state.raitingFilter}
                    onRatingFilterClick={() => this.onRatingFilterClick()}
                />
                <PromptDetail prompt={this.state.selectedItem} onRaitingClick={i => this.onRaitingClick(i)}/>
            </div>
        );
    }
    onKeyDown(e: KeyboardEvent) {
        console.log("Key down: ", e.key);
        const selectedIndex = this.state.items.findIndex(i => i.id == this.state.selectedItem?.id);
        if (e.key == "ArrowLeft") {
            if (selectedIndex > 0) {
                this.onItemSelected(this.state.items[selectedIndex - 1]);
            }
        } else if (e.key == "ArrowRight") {
            if (selectedIndex < this.state.items.length - 1) {
                this.onItemSelected(this.state.items[selectedIndex + 1]);
            }
        } else if (e.key == "f") {
            this.onRaitingClick(this.state.selectedItem);
            e.stopPropagation();
            e.preventDefault();
            e.stopImmediatePropagation();
        }
    }

    async onRatingFilterClick() {
        const raitingFilter = this.state.raitingFilter == 1 ? 0 : 1;
        console.log("settings state: ", raitingFilter)
        this.setState({ raitingFilter }, async () => {
            console.log("settings state done: ", raitingFilter)
            await this.fetchData(raitingFilter);
        });
    }

    async fetchData(raitingFilter: number = 0) {
        const response = await fetch(`api/prompts?raiting=${raitingFilter}`);
        const data = await response.json();
        this.setState({ items: data, loading: false });
        if (data.length > 0) {
            this.onItemSelected(data[0]);
        }
    }

    async onItemSelected(item: any) {
        const response = await fetch(`api/prompts/${item.id}`);
        const data = await response.json();
        data.request = JSON.parse(data.request);
        data.response = JSON.parse(data.response);
        this.setState({ selectedItem: data });
    } 
    
    async onRaitingClick(item: PromptDetailDto) {
        item.raiting = item.raiting == 1 ? 0 : 1;
        const response = await fetch(`api/prompts`, { 
            method: 'PATCH',
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ 
                "id": item.id, 
                "raiting": item.raiting,
            }),
        });
        const data = await response.json();
        item = {...item, ...data};
        this.setState({ selectedItem: item });
    }
}
