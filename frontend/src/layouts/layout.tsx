import Header from '../components/base/header';
import Footer from '../components/base/footer';
import LiveAlerts from '../components/LiveAlerts';

type Props = { children: React.ReactNode };

export default function Layout({ children }: Props) {
  return (
    <div className="app-container">
      <Header />
      <LiveAlerts />
      <main className="app-content">{children}</main>
      <Footer />
    </div>
  );
}
