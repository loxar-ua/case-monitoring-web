import "./clusterDescription.css";

type Props = {
  title: string;
  views: number;
  image?: string;
  description?: string;
};

export default function ClusterDescription({ title, views, image, description }: Props) {
  return (
    <section className="cluster-description">
      <h1 className="cluster-title">{title}</h1>
      <span className="cluster-views">Відвідано: {views}</span>

      {image && (
        <div className="cluster-image-wrapper">
          <img src={image} alt={title} className="cluster-image" />
        </div>
      )}

      {description && (
        <div className="cluster-text-block">
          <p className="cluster-description-text">{description}</p>
          <div className="cluster-ai-footer">Згенеровано ШІ</div>
        </div>
      )}
    </section>
  );
}
