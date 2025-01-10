import { useState } from "react";
import axios from "axios";

const TaskCreate = () => {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [isCompleted, setIsCompleted] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const [successMessage, setSuccessMessage] = useState("");

    const token = localStorage.getItem("jwtToken");
    const apiUrl = "http://localhost:5000/api/tasks";

    const handleSubmit = async (e) => {
        e.preventDefault();

        setLoading(true);
        setError("");
        setSuccessMessage("");

        const taskData = {
            title,
            description,
            isCompleted,
            userId: "user_id_goes_here", // Ge�erli kullan�c� ID'si // varsay�lan olarak sabit
        };

        try {
            const response = await axios.post(apiUrl, taskData, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            setSuccessMessage(`G�rev ba�ar�yla olu�turuldu! G�rev ID: ${response.data.id}`);
            setTitle("");
            setDescription("");
            setIsCompleted(false);
        } catch (err) {
            console.error("Hata:", err);
            setError("G�rev olu�turulurken bir hata olu�tu!");
        } finally {
            setLoading(false);
        }

    };

    return (
        <div>
            <h2>Yeni G�rev Olu�tur</h2>
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
                    {loading ? "Y�kleniyor..." : "G�rev Olu�tur"}
                </button>
            </form>
        </div>
    );
};

export default TaskCreate;
