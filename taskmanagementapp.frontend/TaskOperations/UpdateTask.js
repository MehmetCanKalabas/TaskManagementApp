import { useState, useEffect } from "react";
import axios from "axios";
import { useParams, useHistory } from "react-router-dom";

const TaskUpdate = () => {
    const { id } = useParams();
    const history = useHistory();

    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [isCompleted, setIsCompleted] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const [successMessage, setSuccessMessage] = useState("");

    const token = localStorage.getItem("jwtToken");
    const apiUrl = `http://localhost:5000/api/tasks/${id}`;

    // Task'� y�klemek i�in useEffect
    useEffect(() => {
        const fetchTask = async () => {
            try {
                const response = await axios.get(apiUrl, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });
                setTitle(response.data.title);
                setDescription(response.data.description);
                setIsCompleted(response.data.isCompleted);
            } catch (err) {
                setError(err.response?.data?.message || "G�rev verileri al�n�rken bir hata olu�tu!");
                console.error("Hata:", err); 
            }
        };

        fetchTask();
    }, [apiUrl, token]);


    // G�ncelleme i�lemi
    const handleSubmit = async (e) => {
        e.preventDefault();

        setLoading(true);
        setError("");
        setSuccessMessage("");

        const taskData = {
            title,
            description,
            isCompleted,
            updatedDate: new Date(),
        };

        try {
            const response = await axios.put(apiUrl, taskData, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            setSuccessMessage("G�rev ba�ar�yla g�ncellendi!");
            setTitle(response.data.title);
            setDescription(response.data.description);
            setIsCompleted(response.data.isCompleted);
            history.push("/tasks");
        } catch (err) {
            console.error("Hata:", err);
            setError("G�rev g�ncellenirken bir hata olu�tu!");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h2>G�rev G�ncelle</h2>
            {error && <p style={{ color: "red" }}>{error}</p>}
            {successMessage && <p style={{ color: "green" }}>{successMessage}</p>}
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="title">Ba�l�k</label>
                    <input
                        type="text"
                        id="title"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="description">A��klama</label>
                    <textarea
                        id="description"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>
                        <input
                            type="checkbox"
                            checked={isCompleted}
                            onChange={() => setIsCompleted(!isCompleted)}
                        />
                        Tamamland�
                    </label>
                </div>
                <button type="submit" disabled={loading}>
                    {loading ? "Y�kleniyor..." : "G�revi G�ncelle"}
                </button>
            </form>
        </div>
    );
};

export default TaskUpdate;
