import { useContext } from "react";
import { UserContext } from "../App";

function AuthPage() {
    const { user, setUser } = useContext(UserContext)

    return <div>Hello, {user.name}</div>
}

function LoginWindow({ setUser }) {
    return (
        <div>

        </div>
    );
}

export default HomePage;