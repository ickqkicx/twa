import { useEffect, useState } from "react";
import { HubConnectionBuilder } from '@microsoft/signalr';

export const SimpleChatHub = () => {
    const [connection, setConnection] = useState(null);

    const [userName, setUserName] = useState('');
    const [text, setText] = useState('');
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        if (connection === null) Connect();

        return () => {
            if (connection !== null) {
                connection.stop()
                setConnection(null);
            }
        }

    }, [connection])

    async function Connect() {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:7043/sch')
            .build();

        try {
            await newConnection.start();

            newConnection.on('ReceiveFromGeneral', message => {
                setMessages(prev => [...prev, message])
            })

            setConnection(newConnection);
        } catch (error) {
            console.error('Ошибка при установлении соединения с SignalR:', error);
        }
    }

    async function SendMessage(e) {
        e.preventDefault();
        const message = { userName, text, "time": new Date().toISOString() };

        connection.invoke('SendToGeneral', message)
            .then(() => {
                setText('');
                setMessages(prev => [...prev, message])
            })
            .catch(error => console.error('Ошибка при отправке сообщения:', error))
    }

    return (
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", margin: "30px" }}>
            <input style={{ margin: "10px" }} value={userName} onChange={e => setUserName(e.target.value)} placeholder="Имя пользователя" />

            <div>
                {messages.map(m =>
                    <div style={{ margin: "6px" }} key={m.time}>{m.time}.{m.userName}:{m.text}</div>
                )}
            </div>

            <form onSubmit={SendMessage} style={{ margin: "10px" }}>
                <input value={text} onChange={e => setText(e.target.value)} placeholder="Введите сообщение" />
                <input type="submit" value={"Send"} disabled={connection === null || userName.length < 3 || !text} />
            </form>
        </div>
    )
}
