import { useEffect, useState } from "react";
import axios from "axios";

const TaskList = () => {
    const [tasks, setTasks] = useState([]);
    const [error, setError] = useState("");
    const token = localStorage.getItem("token");

    const apiUrl = "https://api-url/tasks";

    // Task'lar� listele
    useEffect(() => {
        const fetchTasks = async () => {
            try {
                const response = await axios.get(apiUrl, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });
                setTasks(response.data);
            } catch (err) {
                setError("G�rev verileri al�n�rken bir hata olu�tu!");
                console.error("Hata:", err);
            }
        };

        fetchTasks();
    }, []);

    // Task silme i�lemi
    const handleDelete = async (id) => {
        try {
            await axios.delete(`${apiUrl}/${id}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            // Silinen task'� listeden kald�r�yoruz
            setTasks(prevTasks => prevTasks.filter(task => task.id !== id));

            alert("G�rev ba�ar�yla silindi!");
        } catch (err) {
            setError("G�rev silinirken bir hata olu�tu!");
            console.error("Hata:", err);
        }
    };

    return (
        <div>
            <h2>Task Listesi</h2>
            {error && <p>{error}</p>}
            <ul>
                {tasks.map((task) => (
                    <li key={task.id}>
                        <h3>{task.title}</h3>
                        <p>{task.description}</p>
                        <p>{task.isCompleted ? "Tamamland�" : "Tamamlanmad�"}</p>
                        <button onClick={() => handleDelete(task.id)}>Sil</button>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default TaskList;
