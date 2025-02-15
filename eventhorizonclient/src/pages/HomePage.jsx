import { useContext } from "react";
import { UserContext } from "../App";

function HomePage() {
    const { user, setUser } = useContext(UserContext)

    return <div>Hello, {user.name}</div>
}

export default HomePage;