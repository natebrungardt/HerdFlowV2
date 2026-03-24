import { Link } from "react-router-dom";

function Navbar() {
  return (
    <div className="navbar">
      <div className="navbar-left">HerdFlow</div>
      <div className="navbar-links">
        <Link to="/">Dashboard</Link>
        <Link to="/cows">Herd</Link>
        <Link to="/breeding">Breeding</Link>
        <Link to="/health">Health</Link>
        <Link to="/finances">Finances</Link>
      </div>
    </div>
  );
}
export default Navbar;
