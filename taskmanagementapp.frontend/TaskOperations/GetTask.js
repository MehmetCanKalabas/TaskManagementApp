import { useEffect, useState } from "react";
import axios from "axios";

const TaskList = () => {
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const token = localStorage.getItem("jwtToken"); 
    const apiUrl = "http://localhost:5000/api/tasks"; 

    useEffect(() => {
        const fetchTasks = async () => {
            try {
                const response = await axios.get(apiUrl, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });
                setTasks(response.data);
                setLoading(false);
            } catch (error) {
                console.error("API Hatasý:", error);
                setError("Bir hata oluþtu!");
                setLoading(false);
            }
        };

        fetchTasks();
    }, [token]);

    if (loading) return <p>Yükleniyor...</p>;
    if (error) return <p>{error}</p>;

    return (
        <div>
            <h2>Görevler</h2>
            <ul>
                {tasks.map((task) => (
                    <li key={task.id}>
                        <strong>{task.title}</strong>
                        <p>{task.description}</p>
                        <p>{task.isCompleted ? "Tamamlandý" : "Devam Ediyor"}</p>
                        <p>Oluþturulma Tarihi: {task.createdDate}</p>
                        <p>Güncellenme Tarihi: {task.updatedDate}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default TaskList;
