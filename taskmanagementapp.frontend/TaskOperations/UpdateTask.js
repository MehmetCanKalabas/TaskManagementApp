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

    // Task'ý yüklemek için useEffect
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
                setError(err.response?.data?.message || "Görev verileri alýnýrken bir hata oluþtu!");
                console.error("Hata:", err); 
            }
        };

        fetchTask();
    }, [apiUrl, token]);


    // Güncelleme iþlemi
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
            setSuccessMessage("Görev baþarýyla güncellendi!");
            setTitle(response.data.title);
            setDescription(response.data.description);
            setIsCompleted(response.data.isCompleted);
            history.push("/tasks");
        } catch (err) {
            console.error("Hata:", err);
            setError("Görev güncellenirken bir hata oluþtu!");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h2>Görev Güncelle</h2>
            {error && <p style={{ color: "red" }}>{error}</p>}
            {successMessage && <p style={{ color: "green" }}>{successMessage}</p>}
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="title">Baþlýk</label>
                    <input
                        type="text"
                        id="title"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="description">Açýklama</label>
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
                        Tamamlandý
                    </label>
                </div>
                <button type="submit" disabled={loading}>
                    {loading ? "Yükleniyor..." : "Görevi Güncelle"}
                </button>
            </form>
        </div>
    );
};

export default TaskUpdate;
