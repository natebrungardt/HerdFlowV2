import { Link } from "react-router-dom";

export default function LandingFooter() {
  return (
    <footer className="landingFooter" aria-label="Landing page footer">
      <div className="landingFooterBrand">
        <img
          className="landingFooterLogo"
          src="/herdflow-mark.svg"
          alt="HerdFlow logo"
        />
        <div className="landingFooterCopy">
          <span className="landingFooterName">HerdFlow</span>
          <p className="landingFooterTagline">Built for working ranches.</p>
        </div>
      </div>

      <div className="landingFooterMeta">
        <p className="landingFooterCopyright">
          © 2026 HerdFlow. Ranch records without the drag.
        </p>
        <div className="landingFooterLinks">
          <Link className="landingFooterLink" to="/auth?mode=login">
            Sign In
          </Link>
          <Link
            className="landingFooterLink landingFooterButton"
            to="/auth?mode=signup"
          >
            Get Started
          </Link>
        </div>
      </div>
    </footer>
  );
}
