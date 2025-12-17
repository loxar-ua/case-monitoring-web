import "./clusterDescription.css";

type Props = {
  title: string;
  views: number;
  image?: string;
  summary?: string;
};

export default function ClusterDescription({ title, views, image, summary }: Props) {
  return (
    <section className="cluster-description">
      <h1 className="cluster-title">{title}</h1>
      <span className="cluster-views">Відвідано: {views}</span>

      {image && (
        <div className="cluster-image-wrapper">
          <img src={image} alt={title} className="cluster-image" />
        </div>
      )}

      {summary && (
        <div className="cluster-text-block">
           <div
      className="cluster-description-text"
      dangerouslySetInnerHTML={{ __html: summary }}
    ></div>
          <div className="cluster-ai-footer">Згенеровано ШІ</div>
        </div>
      )}
    </section>
  );
}
