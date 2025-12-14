import Header from '../components/base/header';
import Footer from '../components/base/footer';

type Props = { children: React.ReactNode };

export default function Layout({ children }: Props) {
  return (
    <>
      <Header />
      <main>{children}</main>
      <Footer />
    </>
  );
}
