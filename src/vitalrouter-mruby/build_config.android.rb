MRuby::CrossBuild.new('android-arm64') do |conf|
  toolchain :android, arch: 'arm64-v8a'  
  conf.gembox '../../../vitalrouter'
end

MRuby::CrossBuild.new('android-x64') do |conf|
  toolchain :android, arch: 'x86_64'
  conf.gembox '../../../vitalrouter'
end
