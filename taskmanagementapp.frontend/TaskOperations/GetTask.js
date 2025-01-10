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
                console.error("API Hatas�:", error);
                setError("Bir hata olu�tu!");
                setLoading(false);
            }
        };

        fetchTasks();
    }, [token]);

    if (loading) return <p>Y�kleniyor...</p>;
    if (error) return <p>{error}</p>;

    return (
        <div>
            <h2>G�revler</h2>
            <ul>
                {tasks.map((task) => (
                    <li key={task.id}>
                        <strong>{task.title}</strong>
                        <p>{task.description}</p>
                        <p>{task.isCompleted ? "Tamamland�" : "Devam Ediyor"}</p>
                        <p>Olu�turulma Tarihi: {task.createdDate}</p>
                        <p>G�ncellenme Tarihi: {task.updatedDate}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default TaskList;
