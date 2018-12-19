import { get } from './httpRequest';

export default async ns => {
    const getEdges = (elements) => {
        return elements
            .filter(e => e.fromNodeId || e.fromNodeLabel)
            .map(e => ({
                data: {
                    source:
                        e.fromNodeId
                            ? e.fromNodeId
                            : elements.filter(i => i.data.label === e.fromNodeLabel)[0].data.id,
                    target: e.data.id,
                    label: e.data.labelData
                        ? `${e.data.labelData.connectionCount} connections`
                        : ''
                }
            }));
    };
    const getUrls = (s, agregatedData) => {
        return s.routes
            .map(r => r.url)
            .filter((u, i, ar) => ar.indexOf(u) === i)
            .reduce((ar, u) => {
                ar.push({
                    data: {
                        id: `url_${agregatedData.urlIndex++}`,
                        label: u
                    },
                    fromNodeId: `site_${agregatedData.siteIndex}`
                });
                return ar;
            }, []);
    };
    const getRoutes = (s, agregatedData) => {
        const totalConnections = Object.keys(sessions).reduce((a, k) => a + sessions[k], 0);
        return s.routes
            .map(r => ({
                data: {
                    id: `route_${agregatedData.routeIndex++}`,
                    label: r.name,
                    routeData: { ...r, host: s.host },
                    labelData: {
                        cookieCount: r.cookieMustContain.filter(c => c[0] !== 'p').length,
                        connectionCount: sessions[s.host + r.name]
                    },
                    weight: sessions[s.host + r.name] / totalConnections
                },
                fromNodeLabel: r.url
            }));
    };

    const sites = await get(`/siteinfo/${ns}`);
    const sessions = await get(`/siteinfo/sessionsPerRoute/${ns}`);
    const { elements } = sites.reduce((a, s) => {
        const siteElements = [];
        siteElements.push({ data: { id: `site_${a.siteIndex}`, label: s.host } });
        siteElements.push(...getUrls(s, a));
        siteElements.push(...getRoutes(s, a));
        siteElements.push(...getEdges(siteElements));
        a.siteIndex++;
        a.elements = [...a.elements, ...siteElements];
        return a;
    }, {
            elements: [],
            siteIndex: 0,
            urlIndex: 0,
            routeIndex: 0
        });
    return elements;
};