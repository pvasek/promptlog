import React, { Component } from 'react';
import { FaHeart, FaRegHeart } from "react-icons/fa";

export function Raiting(props: { raiting: number, onClick: () => void }) {
    return (
        <div className="raiting" onClick={e => props.onClick()}>
            {props.raiting == 0 ? <FaRegHeart/> : <FaHeart/> }
        </div>
    );
}