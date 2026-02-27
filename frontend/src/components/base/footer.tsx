import "./footer.css";

export default function Footer() {
  return (
    <footer className="app-footer">
      <div className="footer-inner">
        <div className="footer-socials">
          <a href="https://facebook.com" target="_blank">Facebook</a>
          <a href="https://twitter.com" target="_blank">Twitter</a>
          <a href="https://instagram.com" target="_blank">Instagram</a>
        </div>
        <div className="footer-copy">
          <p>©2026</p>
        </div>
      </div>
    </footer>
  );
}
