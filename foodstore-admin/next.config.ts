import type { NextConfig } from "next";

const backendUrl = process.env.API_PROXY_URL || process.env.NEXT_PUBLIC_API_URL || "http://localhost:8080/v2";

const nextConfig: NextConfig = {
  output: "standalone",
  experimental: {
    useTypeScriptCli: true,
  },
  async rewrites() {
    return [
      {
        source: "/api/proxy/hubs/:path*",
        destination: `${backendUrl}/hubs/:path*`,
      },
      {
        source: "/api/proxy/:path*",
        destination: `${backendUrl}/api/:path*`,
      },
    ];
  },
};

export default nextConfig;
