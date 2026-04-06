import { Link } from "react-router-dom";
import LandingFooter from "../components/shared/LandingFooter";

export default function LandingPage() {
  return (
    <div className="landingPage">
      <div className="landingBackdrop" aria-hidden="true" />
      <header className="landingHeader">
        <Link className="landingBrand" to="/">
          <img
            className="landingBrandLogo"
            src="/herdflow-mark.svg"
            alt="HerdFlow logo"
          />
          <div className="landingBrandCopy">
            <span className="landingBrandName">HerdFlow</span>
            <span className="landingBrandTag">
              Ranch records without the drag
            </span>
          </div>
        </Link>

        <div className="landingHeaderActions">
          <Link className="landingHeaderLink" to="/auth?mode=login">
            Sign In
          </Link>
          <Link className="landingHeaderButton" to="/auth?mode=signup">
            Get Started
          </Link>
        </div>
      </header>

      <main className="landingHero">
        <section className="landingHeroContent">
          <div className="landingEyebrow">Built for working ranches</div>
          <div className="landingPain">
            Most ranch software is complicated. This isn't.
          </div>
          <h1 className="landingTitle">
            Finally, run your ranch without the complexity.
          </h1>
          <p className="landingLead">
            Most cattle tracking tools are hard to use in the real world.
            HerdFlow keeps it simple — track cattle, log workdays, and capture
            notes without the headache.
          </p>

          <div className="landingActions">
            <Link className="landingPrimaryButton" to="/auth?mode=signup">
              Start Tracking Your Herd
            </Link>
            <Link className="landingSecondaryButton" to="/auth?mode=login">
              Sign In
            </Link>
          </div>

          <p className="landingPlatformNote">iOS coming soon</p>
          <p className="landingSimpleNote">
            No training. No clutter. Just works.
          </p>

          <div className="landingFeaturePills" aria-label="Key benefits">
            <span className="landingFeaturePill">Herd records</span>
            <span className="landingFeaturePill">Workday planning</span>
            <span className="landingFeaturePill">Crew-ready updates</span>
          </div>
        </section>

        <section className="landingShowcase" aria-label="HerdFlow overview">
          <div className="landingShowcaseCard">
            <div className="landingShowcaseTop">
              <div className="landingShowcaseBadge">Today in HerdFlow</div>
              <div className="landingShowcaseStat">
                <strong>60+</strong>
                <span>cattle actively tracked</span>
              </div>
            </div>

            <div className="landingShowcaseList">
              <article className="landingShowcaseItem">
                <span className="landingShowcaseLabel">Morning pass</span>
                <strong>
                  Update notes from the field before they get lost.
                </strong>
              </article>
              <article className="landingShowcaseItem">
                <span className="landingShowcaseLabel">Crew alignment</span>
                <strong>Keep workdays visible for everyone heading out.</strong>
              </article>
              <article className="landingShowcaseItem">
                <span className="landingShowcaseLabel">
                  History that sticks
                </span>
                <strong>
                  Archive past records without spreadsheet sprawl.
                </strong>
              </article>
            </div>

            <div className="landingShowcaseFooter">
              <img
                className="landingShowcaseLogo"
                src="/herdflow-mark.svg"
                alt=""
              />
              <p>
                One home for the details that keep a ranch operation moving.
              </p>
            </div>
          </div>
        </section>
      </main>
      <LandingFooter />
    </div>
  );
}
